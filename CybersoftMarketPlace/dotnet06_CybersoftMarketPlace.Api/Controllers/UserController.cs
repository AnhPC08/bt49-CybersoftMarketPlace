using System.Threading.Tasks;
using backend_netcore_dotnet06.Helper;  
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace dotnet06_CybersoftMarketPlace.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        //Chỉ làm việc với service, không làm việc trực tiếp với repository
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Tạo user từ view đăng ký tài khoản người dùng
        /// </summary>
        /// <param name="model">Thông tin đăng ký của người dùng</param>
        /// <returns>Trả về kết quả đăng ký</returns>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(HTTPResponseData<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HTTPResponseData<string>))]

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDTO model)
        {
            HTTPResponseData<string>? response = await _userService.RegisterUserAsync(model);
            return StatusCode(response.statusCode, response);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO model)
        {
            HTTPResponseData<string>? response = await _userService.LoginUserAsync(model);
            return StatusCode(response.statusCode, response);
        }


        [Authorize]
        [HttpPost("getProfile")]
        public async Task<IActionResult> GetProfile()
        {
            //Lấy token từ header Authorization
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");


            HTTPResponseData<ProfileUserDTO>? response = await _userService.GetProfileAsync(token);
            return StatusCode(response.statusCode, response);
        }

        /// <summary>
        /// Cập nhật hồ sơ người dùng đang đăng nhập (họ tên, email, SĐT, địa chỉ).
        /// </summary>
        /// <param name="model">Các field hồ sơ cho phép sửa (không gồm Id, không gồm ảnh)</param>
        /// <returns>Hồ sơ sau khi cập nhật</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HTTPResponseData<ProfileUserDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HTTPResponseData<ProfileUserDTO>))]
        [Authorize]
        [HttpPut("updateProfile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDTO model)
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            HTTPResponseData<ProfileUserDTO>? response = await _userService.UpdateProfileAsync(token, model);
            return StatusCode(response.statusCode, response);
        }

        /// <summary>
        /// Đổi mật khẩu người dùng đang đăng nhập (bắt buộc nhập đúng mật khẩu hiện tại).
        /// </summary>
        /// <param name="model">Mật khẩu cũ, mật khẩu mới và xác nhận mật khẩu mới</param>
        /// <returns>Thông báo kết quả đổi mật khẩu</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HTTPResponseData<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HTTPResponseData<string>))]
        [Authorize]
        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            HTTPResponseData<string>? response = await _userService.ChangePasswordAsync(token, model);
            return StatusCode(response.statusCode, response);
        }

        /// <summary>
        /// Cập nhật ảnh đại diện (dán link ảnh) cho người dùng đang đăng nhập.
        /// </summary>
        /// <param name="model">Đường dẫn ảnh đại diện mới</param>
        /// <returns>Thông báo kết quả cập nhật ảnh</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HTTPResponseData<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(HTTPResponseData<string>))]
        [Authorize]
        [HttpPut("updateAvatar")]
        public async Task<IActionResult> UpdateAvatar(UpdateAvatarDTO model)
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            HTTPResponseData<string>? response = await _userService.UpdateAvatarAsync(token, model);
            return StatusCode(response.statusCode, response);
        }



    }
}
