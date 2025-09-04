using AutoMapper;
using BookBuddyAPI.Models.Domain;
using BookBuddyAPI.Models.DTO;
namespace BookBuddyAPI.Mappings
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, AddUserDTO>().ReverseMap();
            CreateMap<UserBook, UserBookDTO>().ReverseMap();
            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<Book, AddBookDTO>().ReverseMap();
            CreateMap<BuddyRequest, BuddyRequestDTO>().ReverseMap();
            CreateMap<Buddy, BuddyDTO>().ReverseMap();
            CreateMap<Notification, NotificationDTO>().ReverseMap();
            CreateMap<Conversation, ConversationDTO>().ReverseMap();
            CreateMap<Conversation, AddConversationDTO>().ReverseMap();
            CreateMap<ConversationMember, ConversationMemberDTO>().ReverseMap();
            CreateMap<ConversationMember, AddConversationMemberDTO>().ReverseMap();
            CreateMap<Message, AddMessageDTO>().ReverseMap();
            CreateMap<Message, MessageDTO>().ReverseMap();
            CreateMap<MessageReaction, MessageReactionDTO>().ReverseMap();
        }
    }
}
