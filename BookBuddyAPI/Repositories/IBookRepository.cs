using BookBuddyAPI.Models.Domain;

namespace BookBuddyAPI.Repositories
{
    public interface IBookRepository
    {
        Task<Book?> GetBookByIdAsync(Guid id);
        Task<Book?> GetBookByAuthorTitleAsync(string author, string title);
        Task<Book?> CreateAsync(Book? book);
        Task<Book?> UpdateAsync(Book? book);
        Task<UserBook> CreateUserBookAsync(UserBook userBook);
        Task<UserBook?> DeleteUserBookAsync(Guid userId, Guid bookId, BookType type);
        Task<BookRating> AddRatingAsync(BookRating rating);
        Task<BookReview> AddReviewAsync(BookReview review);
        Task<BookReview?> UpdateReviewAsync(UpdateReview review);
        Task<bool> DeleteReviewAsync(Guid Id);
        Task<bool> DeleteRatingAsync(Guid userId, Guid bookId);
        Task<List<BookReview>> GetBookReviewsAsync(Guid bookId);

    }
}
