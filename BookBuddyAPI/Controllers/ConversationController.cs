using AutoMapper;
using BookBuddyAPI.Models.Domain;
using BookBuddyAPI.Models.DTO;
using BookBuddyAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationRepository conversationRepository;
        private readonly IMapper mapper;

        public ConversationController(IConversationRepository conversationRepository, IMapper mapper)
        {
            this.conversationRepository = conversationRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        [Route("{conversationId}")]
        // GET: api/conversation/{conversationId}
        public async Task<IActionResult> GetConversation([FromRoute] string conversationId)
        {
            var id = Guid.Parse(conversationId);
            var conversationDomainModel = await conversationRepository.GetConversationAsync(id);
            if (conversationDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<ConversationDTO>(conversationDomainModel));
        }
        // POST api/conversation
        [HttpPost]
        public async Task<IActionResult> CreateConversation([FromBody] AddConversationDTO conversationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var conversation = mapper.Map<Conversation>(conversationDto);

            var createdConversation = await conversationRepository.CreateConversationAsync(conversation);
            if (createdConversation == null)
                return BadRequest("Failed to create conversation.");

            var resultDto = mapper.Map<ConversationDTO>(createdConversation);
            return Ok(resultDto);
        }

        // PUT api/conversation/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConversation(string conversationId, [FromBody] AddConversationDTO conversationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = Guid.Parse(conversationId);

            var conversationToUpdate = mapper.Map<Conversation>(conversationDto);
            conversationToUpdate.Id = id; // Ensure ID consistency

            var updatedConversation = await conversationRepository.UpdateConversationAsync(conversationToUpdate);

            if (updatedConversation == null)
                return NotFound();

            var resultDto = mapper.Map<ConversationDTO>(updatedConversation);
            return Ok(resultDto);
        }

        // DELETE api/conversation/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConversation(string conversationId)
        {
            var id = Guid.Parse(conversationId);
            var deleted = await conversationRepository.DeleteConversationAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
