//Nơi định nghĩa các phương thức truy xuất dữ liệu liên quan đến Customer

using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface ICustomerRepository : IRepositoryBase<Customer>
{
    //Đã có các method của interface từ IRepositoryBase, nếu muốn thêm các method đặc thù cho Customer thì khai báo ở đây
}

public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
{
    public CustomerRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
