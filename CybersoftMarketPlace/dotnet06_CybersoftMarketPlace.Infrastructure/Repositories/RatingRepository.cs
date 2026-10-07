//Nơi định nghĩa các phương thức truy xuất dữ liệu liên quan đến Rating

using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface IRatingRepository : IRepositoryBase<Rating>
{
    //Đã có các method của interface từ IRepositoryBase, nếu muốn thêm các method đặc thù cho Rating thì khai báo ở đây
}

public class RatingRepository : RepositoryBase<Rating>, IRatingRepository
{
    public RatingRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
