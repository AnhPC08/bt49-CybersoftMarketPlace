//Nơi định nghĩa các phương thức truy xuất dữ liệu liên quan đến Message

using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface IMessageRepository : IRepositoryBase<Message>
{
    //Đã có các method của interface từ IRepositoryBase, nếu muốn thêm các method đặc thù cho Message thì khai báo ở đây
}

public class MessageRepository : RepositoryBase<Message>, IMessageRepository
{
    public MessageRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
