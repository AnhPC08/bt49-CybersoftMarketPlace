//Nơi định nghĩa các phương thức truy xuất dữ liệu liên quan đến ProductVariant

using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface IProductVariantRepository : IRepositoryBase<ProductVariant>
{
    //Đã có các method của interface từ IRepositoryBase, nếu muốn thêm các method đặc thù cho ProductVariant thì khai báo ở đây
}

public class ProductVariantRepository : RepositoryBase<ProductVariant>, IProductVariantRepository
{
    public ProductVariantRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
