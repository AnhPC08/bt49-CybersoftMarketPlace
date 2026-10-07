//Nơi định nghĩa các phương thức truy xuất dữ liệu liên quan đến Role

using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface IRoleRepository : IRepositoryBase<Role>
{
    //Đã có các method của interface từ IRepositoryBase, nếu muốn thêm các method đặc thù cho Role thì khai báo ở đây
}

public class RoleRepository : RepositoryBase<Role>, IRoleRepository
{
    public RoleRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
