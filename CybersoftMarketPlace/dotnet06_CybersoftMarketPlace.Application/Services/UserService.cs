


using backend_netcore_dotnet06.Helper;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
public interface IUserService
{
    public Task<HTTPResponseData<string>> RegisterUserAsync(UserRegisterDTO model);
    public Task<HTTPResponseData<string>> LoginUserAsync(UserLoginDTO model);
    public Task<HTTPResponseData<ProfileUserDTO>> GetProfileAsync(string token);
    public Task<HTTPResponseData<ProfileUserDTO>> UpdateProfileAsync(string token, UpdateProfileDTO model);
    public Task<HTTPResponseData<string>> ChangePasswordAsync(string token, ChangePasswordDTO model);
    public Task<HTTPResponseData<string>> UpdateAvatarAsync(string token, UpdateAvatarDTO model);
}

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly JwtAuthService _jwtAuthService;
    private readonly IUserRepository _userRepository;

    //Trong tầng service sẽ gọi các repository để xử lý

    public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository, IUserRoleRepository userRoleRepository,JwtAuthService jwtAuthService)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _jwtAuthService = jwtAuthService;
    }

    public async Task<HTTPResponseData<ProfileUserDTO>> GetProfileAsync(string token)
    {
        try
        {
            //decode token lấy userid
            string? userId = _jwtAuthService.DecodePayloadTokenId(token);
            if (userId == null)
            {
                return new HTTPResponseData<ProfileUserDTO>
                {
                    DataResponse = null,
                    Message = "Token không hợp lệ",
                    statusCode = 401,
                    Timestamp = DateTime.Now
                };
            }
            //Nếu token hợp lệ  thì dựa vào userid lấy thông tin user đưa vào dtoprofile
            User? user = await _unitOfWork.UserRepository.SingleOrDefault(u => u.Id.ToString() == userId);
            ProfileUserDTO profileUserDTO = new ProfileUserDTO
            {
                Id = user.Id.ToString(),
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.Phone,
                Address = user.Address,
                AvatarUrl = user.Avatar
            };
            return new HTTPResponseData<ProfileUserDTO>
            {
                DataResponse = profileUserDTO,
                Message = "Lấy thông tin người dùng thành công",
                statusCode = 200,
                Timestamp = DateTime.Now
            };

            //Dùng userid để lấy thông tin user từ database
        }catch (Exception ex)
        {
            return new HTTPResponseData<ProfileUserDTO>
            {
                DataResponse = null,
                Message = "Lỗi khi lấy thông tin người dùng: " + ex.Message,
                statusCode = 401,
                Timestamp = DateTime.Now
            };
        }
      

    }

    public async Task<HTTPResponseData<string>> LoginUserAsync(UserLoginDTO model)
    {


        // tạo token
        string? token = await _jwtAuthService.GenerateToken(model);
        if(token == null)
        {
            return new HTTPResponseData<string>
            {
                DataResponse = UserResponseMessageDTO.UserNotFound,
                Message = UserResponseMessageDTO.UserNotFound,
                statusCode = 401,
                Timestamp = DateTime.Now
            };
        }

        return new HTTPResponseData<string>
        {
            DataResponse = token,
            Message = UserResponseMessageDTO.SuccessLogin,
            statusCode = 200,
            Timestamp = DateTime.Now
        };
    }

    public async Task<HTTPResponseData<string>> RegisterUserAsync(UserRegisterDTO model)
    {
        try
        {
            // 1/ insert vào bảng user
            User userModel = new User
            {
                Id = Guid.NewGuid(),
                Username = model.Username,
                FullName = model.FullName,
                Alias = HelperFunction.StringToSlug(model.FullName),
                Email = model.Email,
                Phone = model.Phone,
                Avatar = @$"https://ui-avatars.com/api/?name={model.FullName}&background=random&size=128",
                PasswordHash = HelperFunction.HashPassword(model.Password),
                Address = model.Address,
                CreatedAt = DateTime.Now,
                Deleted = false,
            };

            //1.1 check email trùng
            var existingUserByEmail = await _userRepository.SingleOrDefault(u => u.Email == model.Email || u.Username == model.Username || u.Phone == model.Phone);
            if (existingUserByEmail != null)
            {
                return new HTTPResponseData<string>
                {
                    DataResponse = UserResponseMessageDTO.EmailUsernameOrPhoneExists,
                    Message = UserResponseMessageDTO.EmailUsernameOrPhoneExists,
                    statusCode = 400,
                    Timestamp = DateTime.Now
                };
            }

            await _userRepository.AddAsync(userModel);
            //Add liên bảng userole
            UserRole userRoleModel = new UserRole
            {

                UserId = userModel.Id,
                RoleId = UserRoleConst.User, //Mặc định khi đăng ký sẽ là role user
            };
            userModel.UserRoles.Add(userRoleModel);
            //Sau khi thêm userrole từ usermodel thì savachangesAsync sẽ tự động thêm vào bảng userrole
            await _unitOfWork.SaveChangesAsync();
            return new HTTPResponseData<string>
            {
                DataResponse = UserResponseMessageDTO.SuccessRegister,
                Message = UserResponseMessageDTO.SuccessRegister,
                statusCode = 201,
                Timestamp = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
        }
            return new HTTPResponseData<string>
            {
                DataResponse = UserResponseMessageDTO.FailedRegister,
                Message = UserResponseMessageDTO.FailedRegister,
                statusCode = 400,
                Timestamp = DateTime.Now
            };

    }

    // Giải mã token lấy userId. Trả về null nếu token hỏng/thiếu (controller đã chặn [Authorize]
    // nhưng vẫn phòng trường hợp token không đọc được payload).
    private Guid? GetUserIdFromToken(string token)
    {
        try
        {
            string? userId = _jwtAuthService.DecodePayloadTokenId(token);
            if (string.IsNullOrEmpty(userId)) return null;
            return Guid.TryParse(userId, out Guid id) ? id : (Guid?)null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<HTTPResponseData<ProfileUserDTO>> UpdateProfileAsync(string token, UpdateProfileDTO model)
    {
        // 1/ Lấy Id người đang đăng nhập từ token (KHÔNG nhận Id từ client)
        Guid? userId = GetUserIdFromToken(token);
        if (userId == null)
        {
            return new HTTPResponseData<ProfileUserDTO>
            {
                DataResponse = null,
                Message = UserResponseMessageDTO.InvalidToken,
                statusCode = 401,
                Timestamp = DateTime.Now
            };
        }

        // 2/ Validate dữ liệu vào
        if (string.IsNullOrWhiteSpace(model.FullName))
        {
            return new HTTPResponseData<ProfileUserDTO>
            {
                DataResponse = null,
                Message = UserResponseMessageDTO.FullNameRequired,
                statusCode = 400,
                Timestamp = DateTime.Now
            };
        }
        if (string.IsNullOrWhiteSpace(model.Email))
        {
            return new HTTPResponseData<ProfileUserDTO>
            {
                DataResponse = null,
                Message = UserResponseMessageDTO.EmailRequired,
                statusCode = 400,
                Timestamp = DateTime.Now
            };
        }

        // 3/ Lấy user (bản ghi được context theo dõi để lát nữa SaveChanges là update)
        User? user = await _unitOfWork.UserRepository.SingleOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return new HTTPResponseData<ProfileUserDTO>
            {
                DataResponse = null,
                Message = UserResponseMessageDTO.UserNotExist,
                statusCode = 404,
                Timestamp = DateTime.Now
            };
        }

        // 4/ Email cho sửa -> phải check trùng (cả Phone) với tài khoản KHÁC
        User? duplicated = await _unitOfWork.UserRepository.SingleOrDefault(u =>
            u.Id != userId &&
            (u.Email == model.Email || (model.PhoneNumber != null && u.Phone == model.PhoneNumber)));
        if (duplicated != null)
        {
            return new HTTPResponseData<ProfileUserDTO>
            {
                DataResponse = null,
                Message = UserResponseMessageDTO.EmailOrPhoneExists,
                statusCode = 400,
                Timestamp = DateTime.Now
            };
        }

        // 5/ Cập nhật đúng các field cho phép sửa
        user.FullName = model.FullName.Trim();
        user.Email = model.Email.Trim();
        user.Phone = model.PhoneNumber;
        user.Address = model.Address;
        user.Alias = HelperFunction.StringToSlug(model.FullName);
        await _unitOfWork.SaveChangesAsync();

        ProfileUserDTO profile = new ProfileUserDTO
        {
            Id = user.Id.ToString(),
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.Phone,
            Address = user.Address,
            AvatarUrl = user.Avatar
        };
        return new HTTPResponseData<ProfileUserDTO>
        {
            DataResponse = profile,
            Message = UserResponseMessageDTO.UpdateProfileSuccess,
            statusCode = 200,
            Timestamp = DateTime.Now
        };
    }

    public async Task<HTTPResponseData<string>> ChangePasswordAsync(string token, ChangePasswordDTO model)
    {
        Guid? userId = GetUserIdFromToken(token);
        if (userId == null)
        {
            return new HTTPResponseData<string>
            {
                DataResponse = UserResponseMessageDTO.InvalidToken,
                Message = UserResponseMessageDTO.InvalidToken,
                statusCode = 401,
                Timestamp = DateTime.Now
            };
        }

        // Validate (BE vẫn kiểm dù FE đã kiểm - không tin dữ liệu client)
        if (string.IsNullOrWhiteSpace(model.OldPassword))
        {
            return new HTTPResponseData<string>
            {
                DataResponse = UserResponseMessageDTO.OldPasswordRequired,
                Message = UserResponseMessageDTO.OldPasswordRequired,
                statusCode = 400,
                Timestamp = DateTime.Now
            };
        }
        if (string.IsNullOrEmpty(model.NewPassword) || model.NewPassword.Length < 6)
        {
            return new HTTPResponseData<string>
            {
                DataResponse = UserResponseMessageDTO.NewPasswordTooShort,
                Message = UserResponseMessageDTO.NewPasswordTooShort,
                statusCode = 400,
                Timestamp = DateTime.Now
            };
        }
        if (model.NewPassword != model.ConfirmPassword)
        {
            return new HTTPResponseData<string>
            {
                DataResponse = UserResponseMessageDTO.ConfirmPasswordMismatch,
                Message = UserResponseMessageDTO.ConfirmPasswordMismatch,
                statusCode = 400,
                Timestamp = DateTime.Now
            };
        }

        User? user = await _unitOfWork.UserRepository.SingleOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return new HTTPResponseData<string>
            {
                DataResponse = UserResponseMessageDTO.UserNotExist,
                Message = UserResponseMessageDTO.UserNotExist,
                statusCode = 404,
                Timestamp = DateTime.Now
            };
        }

        // Đối chiếu mật khẩu cũ bằng BCrypt
        if (!HelperFunction.VerifyPassword(model.OldPassword, user.PasswordHash))
        {
            return new HTTPResponseData<string>
            {
                DataResponse = UserResponseMessageDTO.OldPasswordIncorrect,
                Message = UserResponseMessageDTO.OldPasswordIncorrect,
                statusCode = 400,
                Timestamp = DateTime.Now
            };
        }

        user.PasswordHash = HelperFunction.HashPassword(model.NewPassword);
        await _unitOfWork.SaveChangesAsync();
        return new HTTPResponseData<string>
        {
            DataResponse = UserResponseMessageDTO.ChangePasswordSuccess,
            Message = UserResponseMessageDTO.ChangePasswordSuccess,
            statusCode = 200,
            Timestamp = DateTime.Now
        };
    }

    public async Task<HTTPResponseData<string>> UpdateAvatarAsync(string token, UpdateAvatarDTO model)
    {
        Guid? userId = GetUserIdFromToken(token);
        if (userId == null)
        {
            return new HTTPResponseData<string>
            {
                DataResponse = UserResponseMessageDTO.InvalidToken,
                Message = UserResponseMessageDTO.InvalidToken,
                statusCode = 401,
                Timestamp = DateTime.Now
            };
        }
        if (string.IsNullOrWhiteSpace(model.AvatarUrl))
        {
            return new HTTPResponseData<string>
            {
                DataResponse = UserResponseMessageDTO.AvatarUrlRequired,
                Message = UserResponseMessageDTO.AvatarUrlRequired,
                statusCode = 400,
                Timestamp = DateTime.Now
            };
        }

        User? user = await _unitOfWork.UserRepository.SingleOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return new HTTPResponseData<string>
            {
                DataResponse = UserResponseMessageDTO.UserNotExist,
                Message = UserResponseMessageDTO.UserNotExist,
                statusCode = 404,
                Timestamp = DateTime.Now
            };
        }

        user.Avatar = model.AvatarUrl.Trim();
        await _unitOfWork.SaveChangesAsync();
        return new HTTPResponseData<string>
        {
            DataResponse = UserResponseMessageDTO.UpdateAvatarSuccess,
            Message = UserResponseMessageDTO.UpdateAvatarSuccess,
            statusCode = 200,
            Timestamp = DateTime.Now
        };
    }
}