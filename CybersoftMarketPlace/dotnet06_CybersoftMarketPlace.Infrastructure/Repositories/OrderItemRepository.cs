//Nơi định nghĩa các phương thức truy xuất dữ liệu liên quan đến OrderItem

using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface IOrderItemRepository : IRepositoryBase<OrderItem>
{
    //Đã có các method của interface từ IRepositoryBase, nếu muốn thêm các method đặc thù cho OrderItem thì khai báo ở đây
}

public class OrderItemRepository : RepositoryBase<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
