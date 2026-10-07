//Nơi định nghĩa các phương thức truy xuất dữ liệu liên quan đến ProductImage

using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface IProductImageRepository : IRepositoryBase<ProductImage>
{
    //Đã có các method của interface từ IRepositoryBase, nếu muốn thêm các method đặc thù cho ProductImage thì khai báo ở đây
}

public class ProductImageRepository : RepositoryBase<ProductImage>, IProductImageRepository
{
    public ProductImageRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
