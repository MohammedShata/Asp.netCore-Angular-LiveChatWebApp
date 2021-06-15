using System.Threading.Tasks;
using System.Collections.Generic;
using api.Dto;
using api.Entites;
using api.Helpers;

namespace api.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void deleteMessage(Message message);
        Task<Message> GetMessage(int id);
      Task<PagedList<MessageDto>> GetMessageForUser();
        Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId,int recipientId);
         Task<bool> saveAllAsync();
    }
}