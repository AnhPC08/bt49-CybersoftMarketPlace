//Nơi định nghĩa các phương thức truy xuất dữ liệu liên quan đến Category

using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface ICategoryRepository : IRepositoryBase<Category>
{
    //Đã có các method của interface từ IRepositoryBase, nếu muốn thêm các method đặc thù cho Category thì khai báo ở đây
}

public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
    public CategoryRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
