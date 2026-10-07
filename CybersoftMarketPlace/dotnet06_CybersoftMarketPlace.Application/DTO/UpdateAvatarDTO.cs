// Tách riêng DTO đổi ảnh đại diện (chỉ đúng 1 field).
// Lý do không gộp vào UpdateProfileDTO: khi nâng cấp lên upload file thật (IFormFile),
// avatar sẽ đi bằng multipart chứ không đi chung JSON với các field hồ sơ -> tách sẵn đỡ sửa lại.
public class UpdateAvatarDTO
{
    public string AvatarUrl { get; set; } = null!;
}
