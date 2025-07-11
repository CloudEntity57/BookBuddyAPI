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
        Task<UserBook?> DeleteUserBookAsync(Guid userId, Guid bookId);

    }
}
