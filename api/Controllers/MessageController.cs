using System.Threading.Tasks;
using api.Dto;
using api.Entites;
using api.Extensions;
using api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        
        public IUserRepository _UserRepository { get; }
        public IMessageRepository _MessageReposirtory { get; }
        public IMapper _Mapper { get; }
        public MessagesController(IUserRepository userRepository,
        IMessageRepository messageReposirtory,IMapper mapper)
        {
           _UserRepository = userRepository;
            _MessageReposirtory = messageReposirtory;
            _Mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username= User.GetUsername();

            if(username==createMessageDto.RecipientUsername.ToLower())
            return BadRequest("You cannot send messages to your self");

            var sender = await _UserRepository.GetUserByUsernameAsync(username);
            var recipient=await _UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);
            if(recipient==null) return NotFound();
            var message=new Message
            {
                Sender=sender,
                Recipient=recipient,
                SenderUsername=sender.UserName,
                RecipientUsername=recipient.UserName,
                Content=createMessageDto.Content
            };
            _MessageReposirtory.AddMessage(message);
            if(await _MessageReposirtory.saveAllAsync()) return Ok(_Mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message");
        }

        
    }
}