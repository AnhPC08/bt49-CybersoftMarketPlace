// DTO cho màn "Đơn mua". Làm theo kiểu lồng nhau giống ProductDetailDTO (buổi 48).
// SubTotal và TotalAmount được BE tính sẵn: tiền bạc nên tính một chỗ (server) để
// mọi màn hình hiển thị giống nhau, tránh mỗi nơi FE tự nhân lại ra số lệch nhau.
public class OrderHistoryDTO
{
    public int OrderId { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Đang xử lý"; // bảng Order chưa có cột trạng thái -> mặc định
    public List<OrderHistoryItemDTO> Items { get; set; } = new();
}

public class OrderHistoryItemDTO
{
    public int VariantId { get; set; }
    public string ProductName { get; set; } = null!;  // đi từ Variant.Product.Name
    public string VariantName { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal { get; set; }             // = Quantity * UnitPrice, tính sẵn ở BE
}

// Chi tiết 1 đơn hàng (GET /api/Order/{id}). Hiện dùng chung cấu trúc với lịch sử đơn,
// tách thành class riêng để sau này tiến hoá độc lập (thêm địa chỉ giao, phí ship...).
public class OrderDetailDTO
{
    public int OrderId { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Đang xử lý";
    public List<OrderHistoryItemDTO> Items { get; set; } = new();
}
