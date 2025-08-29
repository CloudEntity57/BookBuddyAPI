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
    public class ConversationMemberController : ControllerBase
    {
        private readonly IConversationMemberRepository conversationMemberRepository;
        private readonly IMapper mapper;

        public ConversationMemberController(IConversationMemberRepository conversationMemberRepository, IMapper mapper)
        {
            this.conversationMemberRepository = conversationMemberRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        [Route("{userId}")]
        // GET: api/conversationMember/{userId}
        public async Task<IActionResult> GetConversationMember([FromRoute] string userId)
        {
            var id = Guid.Parse(userId);
            var conversationMemberDomainModel = await conversationMemberRepository.GetConversationMemberAsync(id);
            if (conversationMemberDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<ConversationMemberDTO>(conversationMemberDomainModel));
        }
        // POST api/conversationMember
        [HttpPost]
        public async Task<IActionResult> CreateConversationMember([FromBody] AddConversationMemberDTO conversationMemberDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var conversationMember = mapper.Map<ConversationMember>(conversationMemberDto);

            var createdConversationMember = await conversationMemberRepository.CreateConversationMemberAsync(conversationMember);
            if (createdConversationMember == null)
                return BadRequest("Failed to create conversationMember.");

            var resultDto = mapper.Map<ConversationMemberDTO>(createdConversationMember);
            return Ok(resultDto);
        }

        // PUT api/conversationMember/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConversationMember(string conversationMemberId, [FromBody] AddConversationMemberDTO conversationMemberDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = Guid.Parse(conversationMemberId);

            var conversationMemberToUpdate = mapper.Map<ConversationMember>(conversationMemberDto);
            conversationMemberToUpdate.UserId = id; // Ensure ID consistency

            var updatedConversationMember = await conversationMemberRepository.UpdateConversationMemberAsync(conversationMemberToUpdate);

            if (updatedConversationMember == null)
                return NotFound();

            var resultDto = mapper.Map<ConversationMemberDTO>(updatedConversationMember);
            return Ok(resultDto);
        }

        // DELETE api/conversationMember/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConversationMember(string conversationMemberId)
        {
            var id = Guid.Parse(conversationMemberId);
            var deleted = await conversationMemberRepository.DeleteConversationMemberAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
