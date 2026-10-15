using BookBuddyAPI.Data;
using BookBuddyAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

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

        public async Task<UserBook?> DeleteUserBookAsync(Guid userId, Guid bookId, BookType type)
        {
            var existingUserBook = await dbContext.UserBook.FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == bookId && x.BookType == type);
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
                    UsersWantToRead = b.UserBookJoin.Where(ubj => ubj.BookType == BookType.WantToRead).Select(ubj => new User
                    {
                        Id = ubj.User.Id,
                        FirstName = ubj.User.FirstName,
                        LastName = ubj.User.LastName,
                        Email = ubj.User.Email,
                        AvatarUrl = ubj.User.AvatarUrl,
                        UserName = ubj.User.UserName,
                        CreatedAt = ubj.User.CreatedAt,
                        LastLoginAt = ubj.User.LastLoginAt,
                        // back end supplies partial link to profile image, and front end uses environment variables to supply domain of the url
                        ProfileImageUrl = ubj.User.ProfileImage != null ? $"/users/profile-image/{ubj.User.Id}" : null
                    }).ToList(),
                }).FirstOrDefaultAsync(x => x.Author.ToLower() == author.ToLower() && x.Title.ToLower() == title.ToLower());

            if (bookDomainModel == null)
            {
                return null;
            }
            return bookDomainModel;
        }

        public async Task<BookRating> AddRatingAsync(BookRating rating)
        {
            await dbContext.BookRatings.AddAsync(rating);
            await dbContext.SaveChangesAsync();
            return rating;
        }

        public async Task<Boolean> DeleteRatingAsync(Guid userId, Guid bookId)
        {
            var existingRating = await dbContext.BookRatings.FirstOrDefaultAsync(x => x.UserId == userId && x.BookId == bookId);
            if (existingRating != null)
            {
                dbContext.BookRatings.Remove(existingRating);
                await dbContext.SaveChangesAsync();
            }
            return true;
        }
        public async Task<BookReview> AddReviewAsync(BookReview review)
        {
            await dbContext.BookReviews.AddAsync(review);
            await dbContext.SaveChangesAsync();
            return review;
        }
        public async Task<BookReview?> UpdateReviewAsync(UpdateReview review)
        {
            var existingReview = await dbContext.BookReviews.FirstOrDefaultAsync(existing => existing.Id == review.Id);
            if(existingReview == null)
            {
                return null;
            }
            existingReview.Review = review.Review;
            existingReview.CreatedAt = DateTime.UtcNow;
            try
            {
                await dbContext.SaveChangesAsync();
                return await dbContext.BookReviews
                .FirstOrDefaultAsync(r => r.Id == review.Id);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteReviewAsync(Guid Id)
        {
            var existingReview = await dbContext.BookReviews.FirstOrDefaultAsync(m => m.Id == Id);

            if (existingReview == null)
                return false;
            try
            {
                dbContext.BookReviews.Remove(existingReview);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false; // In production, log the exception
            }
        }

        public async Task<List<BookReview>> GetBookReviewsAsync(Guid bookId)
        {
            List<BookReview> bookReviews = await dbContext.BookReviews
            .Include(br => br.User)
            .Where(br => br.BookId == bookId)
            .ToListAsync();
            if(bookReviews == null)
            {
                return [];
            };
            return bookReviews;
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
