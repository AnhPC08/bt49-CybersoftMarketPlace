//Nơi định nghĩa các phương thức truy xuất dữ liệu liên quan đến Cart

using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface ICartRepository : IRepositoryBase<Cart>
{
    //Đã có các method của interface từ IRepositoryBase, nếu muốn thêm các method đặc thù cho Cart thì khai báo ở đây
}

public class CartRepository : RepositoryBase<Cart>, ICartRepository
{
    public CartRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
