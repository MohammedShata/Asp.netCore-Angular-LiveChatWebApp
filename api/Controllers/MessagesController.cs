using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dto;
using api.Entites;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    
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
            if(await _MessageReposirtory.SaveAllAsync()) return Ok(_Mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message");
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery]
        MessageParams messsageParams)
        {
            messsageParams.Username=User.GetUsername();
            var messages=await _MessageReposirtory.GetMessageForUser(messsageParams);

            Response.AddPaginationHeader(messages.CurrentPage,messages.PageSize,messages.TotalCount,messages.TotalPages);

            return messages;
        }
[HttpGet("thread/{username}")]
public async Task<ActionResult<IEnumerable<MessageDto>>>GetMessageThread(string username)
{
    var currentUsername=User.GetUsername();

    return Ok(await _MessageReposirtory.GetMessageThread(currentUsername,username));
}
[HttpDelete("{id}")]
public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(int id)
{
    var username=User.GetUsername();
    var message= await _MessageReposirtory.GetMessage(id);
    if(message.Sender.UserName !=username &&
     message.Recipient.UserName != username)
     return Unauthorized();

     if(message.Sender.UserName==username) message.SenderDeleted=true;
     if(message.Recipient.UserName==username) message.RecipientDeleted=true;
     if(message.RecipientDeleted && message.SenderDeleted)
     _MessageReposirtory.deleteMessage(message);

     if(await _MessageReposirtory.SaveAllAsync())return Ok();
     return BadRequest("Problem deleting the message");

}
        
    
}
}