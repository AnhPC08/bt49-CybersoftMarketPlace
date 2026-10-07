// DTO đổi mật khẩu. Bắt buộc có mật khẩu hiện tại để tránh người mượn máy
// (lúc quên đăng xuất) đổi mật khẩu chiếm tài khoản.
// ConfirmPassword được kiểm ở CẢ FE (phản hồi nhanh cho người dùng)
// và BE (không tin client - request có thể bỏ qua FE gửi thẳng vào API).
public class ChangePasswordDTO
{
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}
