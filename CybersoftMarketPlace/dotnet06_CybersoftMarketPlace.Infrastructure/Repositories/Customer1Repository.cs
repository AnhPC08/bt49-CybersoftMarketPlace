//Nơi định nghĩa các phương thức truy xuất dữ liệu liên quan đến Customer1

using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface ICustomer1Repository : IRepositoryBase<Customer1>
{
    //Đã có các method của interface từ IRepositoryBase, nếu muốn thêm các method đặc thù cho Customer1 thì khai báo ở đây
}

public class Customer1Repository : RepositoryBase<Customer1>, ICustomer1Repository
{
    public Customer1Repository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
