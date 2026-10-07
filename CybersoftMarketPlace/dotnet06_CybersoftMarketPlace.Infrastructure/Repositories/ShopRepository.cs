//Nơi định nghĩa các phương thức truy xuất dữ liệu liên quan đến Shop

using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface IShopRepository : IRepositoryBase<Shop>
{
    //Đã có các method của interface từ IRepositoryBase, nếu muốn thêm các method đặc thù cho Shop thì khai báo ở đây
}

public class ShopRepository : RepositoryBase<Shop>, IShopRepository
{
    public ShopRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
