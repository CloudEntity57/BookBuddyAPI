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
        }
    }
}
