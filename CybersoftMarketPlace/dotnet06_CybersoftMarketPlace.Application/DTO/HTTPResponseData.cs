


public class HTTPResponseData<T>
{
    public T DataResponse { get; set; }
    public string Message { get; set; }
    public int statusCode { get; set; }
    public DateTime Timestamp { get; set; }
}


public static class UserResponseMessageDTO
{
    public const string SuccessRegister = "User registered successfully.";
    public const string FailedRegister = "User registration failed.";
    public const string EmailUsernameOrPhoneExists = "Email, username, or phone already exists.";
    public const string SuccessLogin = "User logged in successfully.";
    public const string InvalidLogin = "Invalid email, username, phone, or password.";
    public const string UserNotFound = "Username/Phone/Email or password is incorrect.";

    // --- Buổi 49: cập nhật hồ sơ ---
    public const string InvalidToken = "Token không hợp lệ.";
    public const string UserNotExist = "Không tìm thấy người dùng.";
    public const string FullNameRequired = "Họ và tên không được để trống.";
    public const string EmailRequired = "Email không được để trống.";
    public const string EmailOrPhoneExists = "Email hoặc số điện thoại đã được dùng bởi tài khoản khác.";
    public const string UpdateProfileSuccess = "Cập nhật hồ sơ thành công.";
    public const string UpdateProfileFailed = "Cập nhật hồ sơ thất bại.";

    // --- Buổi 49: đổi mật khẩu ---
    public const string OldPasswordRequired = "Vui lòng nhập mật khẩu hiện tại.";
    public const string NewPasswordTooShort = "Mật khẩu mới phải có ít nhất 6 ký tự.";
    public const string ConfirmPasswordMismatch = "Xác nhận mật khẩu không khớp.";
    public const string OldPasswordIncorrect = "Mật khẩu hiện tại không đúng.";
    public const string ChangePasswordSuccess = "Đổi mật khẩu thành công.";
    public const string ChangePasswordFailed = "Đổi mật khẩu thất bại.";

    // --- Buổi 49: đổi ảnh đại diện ---
    public const string AvatarUrlRequired = "Đường dẫn ảnh không được để trống.";
    public const string UpdateAvatarSuccess = "Cập nhật ảnh đại diện thành công.";
    public const string UpdateAvatarFailed = "Cập nhật ảnh đại diện thất bại.";
}

public static class OrderResponseMessageDTO
{
    public const string InvalidToken = "Token không hợp lệ.";
    public const string GetOrdersSuccess = "Lấy danh sách đơn mua thành công.";
    public const string GetOrderDetailSuccess = "Lấy chi tiết đơn hàng thành công.";
    public const string OrderNotFound = "Không tìm thấy đơn hàng.";
    public const string GetOrdersFailed = "Lấy danh sách đơn mua thất bại.";
}