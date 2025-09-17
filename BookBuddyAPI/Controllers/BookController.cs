using AutoMapper;
using BookBuddyAPI.Models.Domain;
using BookBuddyAPI.Models.DTO;
using BookBuddyAPI.Repositories;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace BookBuddyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ILogger<BookController> logger;
        private readonly IBookRepository repository;

        public BookController(IMapper mapper, ILogger<BookController> logger, IBookRepository repository)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.repository = repository;
        }
        [HttpGet]
        [Route("{author}/{title}")]
        public async Task<IActionResult> GetBookByAuthorTitle([FromRoute] string author, [FromRoute] string title)
        {
            var decodedAuthor = WebUtility.UrlDecode(author);
            var decodedTitle = WebUtility.UrlDecode(title);
            var bookDomainModel = await repository.GetBookByAuthorTitleAsync(decodedAuthor, decodedTitle);
            if (bookDomainModel == null)
            {
                return NotFound();
            }
            var bookDTO = mapper.Map<BookDTO>(bookDomainModel);
            return Ok(bookDTO);
        }
        [HttpPost]
        //  POST: api/Book
        public async Task<IActionResult> AddBookByAuthorTitle([FromBody] AddBookDTO createBookDto)
        {
            var bookDomainModel = mapper.Map<Book>(createBookDto);

            bookDomainModel = await repository.CreateAsync(bookDomainModel);

            var bookDTO = mapper.Map<BookDTO>(bookDomainModel);

            return Ok(bookDomainModel);
        }
        [HttpPut]
        [Route("want_to_read")]
        // PUT: api/Book/want_to_read
        public async Task<IActionResult> UpdateUserBook([FromBody] UserBook userBook)
        {
            //var userBookDto = new UserBook{
            //    UserId = Guid.Parse(userId), 
            //    BookId = Guid.Parse(bookId),
            //    DateAdded = DateTime.UtcNow,
            //    Note = "adding a new book - " + bookId
            //};
            userBook.DateAdded = DateTime.UtcNow;
            var userBookDto = await repository.CreateUserBookAsync(userBook);
            logger.LogInformation("New UserBook Added - " + userBookDto);
            return Ok(userBookDto);
        }
        [HttpDelete]
        [Route("want_to_read/{userId}/{bookId}")]
        // DELETE: api/Book/want_to_read/{userId}/{bookId}
        public async Task<IActionResult> DeleteUserWantToRead([FromRoute] string userId, [FromRoute] string bookId)
        {
            var userGuid = Guid.Parse(userId);
            var bookGuid = Guid.Parse(bookId);
            var deletedUserBookDomainModel = await repository.DeleteUserBookAsync(userGuid, bookGuid, BookType.WantToRead);
            if(deletedUserBookDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<UserBookDTO>(deletedUserBookDomainModel));
        }
    }
}
