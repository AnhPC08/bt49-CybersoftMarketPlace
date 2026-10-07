//Nơi định nghĩa các phương thức truy xuất dữ liệu liên quan đến User

using System.Linq.Expressions;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories;


public interface IUserRepository : IRepositoryBase<User>
{
    //Đã có các method của interface từ IRepositoryBase, nếu muốn thêm các method đặc thù cho User thì khai báo ở đây

    void additionalUserMethod(); // Ví dụ: thêm một phương thức đặc thù cho User

}

public class UserRepository :RepositoryBase<User>, IUserRepository
{
    
    public UserRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }

    public void additionalUserMethod()
    {
        throw new NotImplementedException();
    }

    //Nếu muốn thêm các method đặc thù cho User thì khai báo ở đây
}