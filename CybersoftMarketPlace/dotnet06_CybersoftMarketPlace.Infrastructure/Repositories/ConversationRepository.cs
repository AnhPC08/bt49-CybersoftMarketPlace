//Nơi định nghĩa các phương thức truy xuất dữ liệu liên quan đến Conversation

using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface IConversationRepository : IRepositoryBase<Conversation>
{
    //Đã có các method của interface từ IRepositoryBase, nếu muốn thêm các method đặc thù cho Conversation thì khai báo ở đây
}

public class ConversationRepository : RepositoryBase<Conversation>, IConversationRepository
{
    public ConversationRepository(CybersoftMarketPlaceContext context) : base(context)
    {
    }
}
