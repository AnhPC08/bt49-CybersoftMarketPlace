

using Infrastructure.Models;

public interface IProductRepository : IRepositoryBase<Product>
{
    //Đã có các method của interface từ IRepositoryBase, nếu muốn thêm các method đặc thù cho Product thì khai báo ở đây


}

public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    public ProductRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }



}