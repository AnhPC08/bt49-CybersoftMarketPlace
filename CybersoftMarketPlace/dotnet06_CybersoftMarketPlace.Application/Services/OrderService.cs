
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
public interface IOrderService
{
    Task<HTTPResponseData<string>> CreateOrderAsync(string userId,int cartId);
    Task<HTTPResponseData<List<OrderHistoryDTO>>> GetMyOrdersAsync(string token);
    Task<HTTPResponseData<OrderDetailDTO>> GetOrderDetailAsync(string token, int orderId);
}


public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private readonly JwtAuthService _jwtAuthService;

    public OrderService(IUnitOfWork unitOfWork, IUserService userService, JwtAuthService jwtAuthService)
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
        _jwtAuthService = jwtAuthService;
    }

    public async Task<HTTPResponseData<string>> CreateOrderAsync(string userId, int cartId)
    {
        //1. Kiểm tra giỏ hàng có đang tồn tại không
        var cart = await _unitOfWork.CartRepository.SingleOrDefault(c => c.Id == cartId && c.UserId == Guid.Parse(userId) && c.Deleted == false);
        if (cart == null)
        {
            return new HTTPResponseData<string>
            {
                statusCode = 404,
                Message = "Cart not found.",
                DataResponse = "Cart not found."
            };
        }
        //Cho đặt hàng
        // Lưu order và order item trong 1 lần add vào database
        var order = new Order();
        order.BuyerId = Guid.Parse(userId);
        order.CreatedAt = DateTime.UtcNow;
        order.Deleted = false;

        order.OrderItems = new List<OrderItem>();
        foreach (var cartItem in cart.CartItems)
        {
            var orderItem = new OrderItem
            {
                VariantId = cartItem.VariantId,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.Price ?? 0,
            };
            order.OrderItems.Add(orderItem);
        }

        await _unitOfWork.OrderRepository.AddAsync(order);
        //Xoá giỏ hàng sau khi đặt hàng thành công
        cart.Deleted = true;
        await _unitOfWork.SaveChangesAsync();
        return new HTTPResponseData<string>
        {
            statusCode = 200,
            Message = "Order created successfully.",
            DataResponse = "Order created successfully."
        };



    }

    // Giải mã token lấy userId (đơn hàng chỉ lấy của chính người đăng nhập, không nhận Id từ client).
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

    public async Task<HTTPResponseData<List<OrderHistoryDTO>>> GetMyOrdersAsync(string token)
    {
        Guid? userId = GetUserIdFromToken(token);
        if (userId == null)
        {
            return new HTTPResponseData<List<OrderHistoryDTO>>
            {
                DataResponse = new List<OrderHistoryDTO>(),
                Message = OrderResponseMessageDTO.InvalidToken,
                statusCode = 401,
                Timestamp = DateTime.Now
            };
        }

        // Lấy đơn của chính user. Include để nạp sẵn OrderItems -> Variant -> Product
        // trong CÙNG một truy vấn (tránh N+1 khi lặp qua từng item lấy tên sản phẩm).
        // Màn hình chỉ đọc nên AsNoTracking cho nhẹ (không cần theo dõi thay đổi).
        List<Order> orders = await _unitOfWork.OrderRepository
            .WhereSql(o => o.BuyerId == userId && o.Deleted != true)
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Variant).ThenInclude(v => v.Product)
            .AsNoTracking()
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        List<OrderHistoryDTO> data = orders.Select(MapToHistoryDTO).ToList();

        return new HTTPResponseData<List<OrderHistoryDTO>>
        {
            DataResponse = data,
            Message = OrderResponseMessageDTO.GetOrdersSuccess,
            statusCode = 200,
            Timestamp = DateTime.Now
        };
    }

    public async Task<HTTPResponseData<OrderDetailDTO>> GetOrderDetailAsync(string token, int orderId)
    {
        Guid? userId = GetUserIdFromToken(token);
        if (userId == null)
        {
            return new HTTPResponseData<OrderDetailDTO>
            {
                DataResponse = null,
                Message = OrderResponseMessageDTO.InvalidToken,
                statusCode = 401,
                Timestamp = DateTime.Now
            };
        }

        // Ràng buộc BuyerId == userId ngay trong truy vấn: người này không xem được đơn của người khác.
        Order? order = await _unitOfWork.OrderRepository
            .WhereSql(o => o.Id == orderId && o.BuyerId == userId && o.Deleted != true)
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Variant).ThenInclude(v => v.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (order == null)
        {
            return new HTTPResponseData<OrderDetailDTO>
            {
                DataResponse = null,
                Message = OrderResponseMessageDTO.OrderNotFound,
                statusCode = 404,
                Timestamp = DateTime.Now
            };
        }

        OrderHistoryDTO history = MapToHistoryDTO(order);
        OrderDetailDTO detail = new OrderDetailDTO
        {
            OrderId = history.OrderId,
            CreatedAt = history.CreatedAt,
            TotalAmount = history.TotalAmount,
            Status = history.Status,
            Items = history.Items
        };
        return new HTTPResponseData<OrderDetailDTO>
        {
            DataResponse = detail,
            Message = OrderResponseMessageDTO.GetOrderDetailSuccess,
            statusCode = 200,
            Timestamp = DateTime.Now
        };
    }

    // Map 1 Order (đã Include sẵn) sang DTO. SubTotal & TotalAmount tính ở BE.
    private static OrderHistoryDTO MapToHistoryDTO(Order order)
    {
        List<OrderHistoryItemDTO> items = order.OrderItems.Select(oi => new OrderHistoryItemDTO
        {
            VariantId = oi.VariantId,
            ProductName = oi.Variant.Product.Name,
            VariantName = oi.Variant.VariantName ?? string.Empty,
            ImageUrl = oi.Variant.Image ?? oi.Variant.Product.Image ?? string.Empty,
            Quantity = oi.Quantity,
            UnitPrice = oi.UnitPrice,
            SubTotal = oi.Quantity * oi.UnitPrice
        }).ToList();

        return new OrderHistoryDTO
        {
            OrderId = order.Id,
            CreatedAt = order.CreatedAt ?? default,
            // Cột TotalAmount trong DB có thể chưa được ghi -> lấy tổng từ các dòng hàng cho chắc.
            TotalAmount = order.TotalAmount > 0 ? order.TotalAmount : items.Sum(i => i.SubTotal),
            Status = "Đang xử lý",
            Items = items
        };
    }
}