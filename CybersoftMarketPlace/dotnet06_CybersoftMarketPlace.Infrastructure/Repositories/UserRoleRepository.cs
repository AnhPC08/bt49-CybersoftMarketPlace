
using Infrastructure.Models;
public interface IUserRoleRepository : IRepositoryBase<UserRole>
{
    //Đã có các method của interface từ IRepositoryBase, nếu muốn thêm các method đặc thù cho UserRole thì khai báo ở đây
}

public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
{
    public UserRoleRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}