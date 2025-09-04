using AutoMapper;
using BookBuddyAPI.Models.Domain;
using BookBuddyAPI.Models.DTO;
using BookBuddyAPI.Repositories;
using BookBuddyAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {

        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IMessageService messageService;

        public MessageController(IMessageRepository messageRepository, IMapper mapper, IMessageService messageService)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            this.messageService = messageService;
        }



        // GET: api/message/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessageById(string messageId)
        {
            var id = Guid.Parse(messageId);
            var message = await _messageRepository.GetMessageAsync(id);
            if (message == null)
                return NotFound($"Message with ID {id} not found.");

            return Ok(_mapper.Map<MessageDTO>(message));
        }

        // POST: api/message
        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] AddMessageDTO messageDto)
        {
            if (messageDto == null)
                return BadRequest("Message cannot be null.");

            var message = _mapper.Map<Message>(messageDto);
            var createdMessage = await _messageRepository.CreateMessageAsync(message);

            var resultDto = _mapper.Map<MessageDTO>(createdMessage);

            // update via SignalR in real time:
            await messageService.UpdateMessageAsync(resultDto);

            return Ok(resultDto);
        }

        // PUT: api/message/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(string messageId, [FromBody] AddMessageDTO messageDto)
        {
            var id = Guid.Parse(messageId);
            if (messageDto == null)
                return BadRequest("Message cannot be null.");

            var existingMessage = await _messageRepository.GetMessageAsync(id);
            if (existingMessage == null)
                return NotFound($"Message with ID {id} not found.");

            // Map DTO → existing domain model
           existingMessage = _mapper.Map<Message>(messageDto);

            var updatedMessage = await _messageRepository.UpdateMessageAsync(existingMessage);
            if (updatedMessage == null)
                return NotFound($"Unable to update message with ID {id}.");

            var resultDto = _mapper.Map<MessageDTO>(updatedMessage);
            return Ok(resultDto);
        }

        // DELETE: api/message/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(string messageId)
        {
            var id = Guid.Parse(messageId);
            var success = await _messageRepository.DeleteMessageAsync(id);
            if (!success)
                return NotFound($"Message with ID {id} not found.");

            return NoContent();
        }
    }
}
