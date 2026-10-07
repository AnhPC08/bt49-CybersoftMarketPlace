//Nơi định nghĩa các phương thức truy xuất dữ liệu liên quan đến Order

using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface IOrderRepository : IRepositoryBase<Order>
{
    //Đã có các method của interface từ IRepositoryBase, nếu muốn thêm các method đặc thù cho Order thì khai báo ở đây
}

public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    public OrderRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
