using BookBuddyAPI.Data;
using BookBuddyAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookBuddyAPI.Repositories
{
    public class SQLBookRepository : IBookRepository
    {
        private readonly BookBuddyGeneralDbContext dbContext;
        private readonly ILogger logger;

        public SQLBookRepository(BookBuddyGeneralDbContext dbContext, ILogger<SQLBookRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }
        public async Task<Book?> CreateAsync(Book? book)
        {
            await dbContext.Books.AddAsync(book);
            await dbContext.SaveChangesAsync();
            return book;

        }

        public async Task<UserBook> CreateUserBookAsync(UserBook userBook)
        {
            await dbContext.UserBook.AddAsync(userBook);
            await dbContext.SaveChangesAsync();
            return userBook;
        }

        public async Task<UserBook?> DeleteUserBookAsync(Guid userId, Guid bookId)
        {
            var existingUserBook = await dbContext.UserBook.FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == bookId);
            if (existingUserBook == null)
            {
                return null;
            }
            dbContext.UserBook.Remove(existingUserBook);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("DELETED USERBOOK " + existingUserBook);
            return existingUserBook;
        }

        public async Task<Book?> GetBookByAuthorTitleAsync(string author, string title)
        {
            var bookDomainModel = await dbContext.Books
                .Select(b => new Book
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    UsersWantToRead = b.UserBookJoin.Select(ubj => new User
                    {
                        Id = ubj.User.Id,
                        FirstName = ubj.User.FirstName,
                        LastName = ubj.User.LastName,
                        Email = ubj.User.Email,
                        AvatarUrl = ubj.User.AvatarUrl,
                        UserName = ubj.User.UserName,
                        CreatedAt = ubj.User.CreatedAt,
                        LastLoginAt = ubj.User.LastLoginAt
                    }).ToList()
                }).FirstOrDefaultAsync(x => x.Author.ToLower() == author.ToLower() && x.Title.ToLower() == title.ToLower());

            if (bookDomainModel == null)
            {
                return null;
            }
            return bookDomainModel;
        }

        public Task<Book?> GetBookByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Book?> UpdateAsync(Book? book)
        {
            throw new NotImplementedException();
        }
    }
}
