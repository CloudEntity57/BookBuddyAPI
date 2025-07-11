using Microsoft.EntityFrameworkCore;
using BookBuddyAPI.Models.Domain;
using System.Text.Json;
using BookBuddyAPI.Models.DTO;

namespace BookBuddyAPI.Data
{
    public class BookBuddyGeneralDbContext: DbContext
    {
        public BookBuddyGeneralDbContext(DbContextOptions<BookBuddyGeneralDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<UserBook> UserBook { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // seed test data for users
            var users = new List<UserDTO>()
            {
                new UserDTO()
                {
                    Id = new Guid("603e74b2-511a-431c-b9d5-1db379a0eb0e"),
                    UserName = "HelioMoonWave Literati",
                    Email = "joshthewise57@gmail.com",
                    CreatedAt = new DateTime(2025, 6, 23, 17, 40, 28, 901, DateTimeKind.Local).AddTicks(7489)
                }
            };
            modelBuilder.Entity<UserDTO>().HasData(users);
            // seed test data for books
            var books = new List<Book>()
            {
                new Book()
                {
                    Id = new Guid("603e74b2-512a-432c-b9d5-1db379a0eb0e"),
                    Title = "War and Peace",
                    Author = "Leo Tolstoy"
                }
            };
            modelBuilder.Entity<Book>().HasData(books);

            modelBuilder.Entity<UserBook>()
                .HasKey(ub => new { ub.UserId, ub.BookId }); // composite PK
            modelBuilder.Entity<UserBook>()
                .HasOne(ub => ub.User)
                .WithMany(u => u.WantReadJoin)
                .HasForeignKey(ub => ub.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserBook>()
                .HasOne(ub => ub.Book)
                .WithMany(b => b.UserBookJoin)
                .HasForeignKey(ub => ub.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserBook>().HasData(new
            {
                UserId = Guid.Parse("603e74b2-511a-431c-b9d5-1db379a0eb0e"),
                BookId = Guid.Parse("603e74b2-512a-432c-b9d5-1db379a0eb0e"),
                DateAdded = new DateTime(2025, 6, 24, 17, 03, 28, 901, DateTimeKind.Local).AddTicks(7489),
                Note = "Trying out adding a want to read value"
            });

        }
    }
}
