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
        Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername,string recipientUsername);
         Task<bool> SaveAllAsync();
    }
}