using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookBuddyAPI.Data
{
    public class BookBuddyAuthDbContext : IdentityDbContext
    {
        public BookBuddyAuthDbContext(DbContextOptions<BookBuddyAuthDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            var readerRoleId = "4913d02a-2446-4ced-83c8-295217487d19";
            var writerRoleId = "fc3f5d9e-f9ce-4852-b692-928e1ddfefe5";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            };

            //seed the roles inside the builder object

            builder.Entity<IdentityRole>().HasData(roles);
        }

    }
}
