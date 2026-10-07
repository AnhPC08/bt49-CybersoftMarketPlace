//Nơi định nghĩa các phương thức truy xuất dữ liệu liên quan đến CartItem

using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface ICartItemRepository : IRepositoryBase<CartItem>
{
    //Đã có các method của interface từ IRepositoryBase, nếu muốn thêm các method đặc thù cho CartItem thì khai báo ở đây
}

public class CartItemRepository : RepositoryBase<CartItem>, ICartItemRepository
{
    public CartItemRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
