using BookBuddyAPI.Data;
using BookBuddyAPI.Models.Domain;
using BookBuddyAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace BookBuddyAPI.Repositories
{
    public class SQLUserRepository : IUserRepository
    {
        private readonly BookBuddyGeneralDbContext dbContext;
        private readonly ILogger<SQLUserRepository> logger;

        public SQLUserRepository(BookBuddyGeneralDbContext dbContext, ILogger<SQLUserRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }
        public async Task<User> CreateAsync(User user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var userDomainModel = await dbContext.Users
                .Where(x => x.Id == id)
                .Select(u => new User
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    AvatarUrl = u.AvatarUrl,
                    UserName = u.UserName,
                    CreatedAt = u.CreatedAt,
                    LastLoginAt = u.LastLoginAt,
                    WantToRead = u.WantReadJoin.Select(ub => new
                    Book
                    {
                        Id = ub.BookId,
                        Title = ub.Book.Title,
                        Author = ub.Book.Author,
                        UsersWantToRead = ub.Book.UserBookJoin.Select(ubj => new User
                        {
                            Id = ubj.User.Id,
                            FirstName = ubj.User.FirstName,
                            LastName = ubj.User.LastName,
                            Email = ubj.User.Email,
                            AvatarUrl = ubj.User.AvatarUrl,
                            UserName = ubj.User.UserName,
                            CreatedAt = ubj.User.CreatedAt,
                            LastLoginAt = ubj.User.LastLoginAt
                        }).Where(ubj => ubj.Id != u.Id).ToList()
                    }).ToList(),
                    ReceivedBuddyRequests = u.ReceivedBuddyRequestsJoin != null ? u.ReceivedBuddyRequestsJoin.Select(rb => new User
                    {
                        Id = rb.ActiveUserId,
                        FirstName = rb.ActiveUser.FirstName,
                        LastName = rb.ActiveUser.LastName,
                        UserName = rb.ActiveUser.UserName,
                        //Email = rb.ActiveUser.Email,
                        AvatarUrl = rb.ActiveUser.AvatarUrl,
                        CreatedAt = rb.DateAdded,
                        UserMessage = rb.Note,
                        BookOfInterest = rb.BookTitle
                    }).ToList() : null,
                    SentBuddyRequests = u.SentBuddyRequestsJoin != null ? u.SentBuddyRequestsJoin.Select(sb => new User
                    {
                        Id = sb.PassiveUserId,
                        FirstName = sb.PassiveUser.FirstName,
                        LastName = sb.PassiveUser.LastName,
                        UserName = sb.PassiveUser.UserName,
                        //Email = rb.ActiveUser.Email,
                        AvatarUrl = sb.PassiveUser.AvatarUrl,
                        CreatedAt = sb.DateAdded,
                        UserMessage = sb.Note,
                        BookOfInterest = sb.BookTitle
                    }).ToList() : null
                })
                .FirstOrDefaultAsync();

            if (userDomainModel == null)
            {
                return null;
            }
            //userDomainModel.WantToRead?.ForEach( wtr => wtr.Include("Book"))
            return userDomainModel;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var userDomainModel = await dbContext.Users
                .Where(x => x.Email == email)
                .Select(u => new User
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    AvatarUrl = u.AvatarUrl,
                    UserName = u.UserName,
                    CreatedAt = u.CreatedAt,
                    LastLoginAt = u.LastLoginAt,
                    WantToRead =  u.WantReadJoin.Select(ub => new
                    Book
                    {
                        Id = ub.BookId,
                        Title = ub.Book.Title,
                        Author = ub.Book.Author,
                        UsersWantToRead = ub.Book.UserBookJoin.Select(ubj => new User
                        {
                            Id = ubj.User.Id,
                            FirstName = ubj.User.FirstName,
                            LastName = ubj.User.LastName,
                            Email = ubj.User.Email,
                            AvatarUrl = ubj.User.AvatarUrl,
                            UserName = ubj.User.UserName,
                            CreatedAt = ubj.User.CreatedAt,
                            LastLoginAt = ubj.User.LastLoginAt
                        }).Where(ubj => ubj.Id != u.Id).ToList()
                    }).ToList(),
                    ReceivedBuddyRequests = u.ReceivedBuddyRequestsJoin != null ? u.ReceivedBuddyRequestsJoin.Select(rb => new User
                    {
                        Id = rb.ActiveUserId,
                        FirstName = rb.ActiveUser.FirstName,
                        LastName = rb.ActiveUser.LastName,
                        UserName = rb.ActiveUser.UserName,
                        //Email = rb.ActiveUser.Email,
                        AvatarUrl = rb.ActiveUser.AvatarUrl,
                        CreatedAt = rb.DateAdded,
                        UserMessage = rb.Note,
                        BookOfInterest = rb.BookTitle
                    }).ToList() : null,
                    SentBuddyRequests = u.SentBuddyRequestsJoin != null ? u.SentBuddyRequestsJoin.Select(sb => new User
                    {
                        Id = sb.PassiveUserId,
                        FirstName = sb.PassiveUser.FirstName,
                        LastName = sb.PassiveUser.LastName,
                        UserName = sb.PassiveUser.UserName,
                        //Email = rb.ActiveUser.Email,
                        AvatarUrl = sb.PassiveUser.AvatarUrl,
                        CreatedAt = sb.DateAdded,
                        UserMessage = sb.Note,
                        BookOfInterest = sb.BookTitle
                    }).ToList() : null
                })
                .FirstOrDefaultAsync();

            if (userDomainModel == null)
            {
                return null;
            }
            //userDomainModel.WantToRead?.ForEach( wtr => wtr.Include("Book"))
            return userDomainModel;
        }
    }
}
