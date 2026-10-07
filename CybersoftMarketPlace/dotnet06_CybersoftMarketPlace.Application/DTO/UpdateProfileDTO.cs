// DTO nhận vào khi người dùng bấm "Lưu thay đổi" ở màn Hồ sơ của tôi.
// Nguyên tắc: nhìn giao diện để suy ra DTO, chỉ đưa đúng các field cho phép sửa.
// KHÔNG có Id (Id lấy từ token, client gửi Id lên là lỗ hổng chiếm tài khoản người khác).
// KHÔNG có Avatar (ảnh đại diện đi đường riêng qua UpdateAvatarDTO).
public class UpdateProfileDTO
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;   // cho sửa -> phải check trùng như lúc Register
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}
