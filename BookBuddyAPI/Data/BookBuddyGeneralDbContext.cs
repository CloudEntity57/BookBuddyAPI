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

        public DbSet<BuddyRequest> BuddyRequest { get; set; }
        public DbSet<Buddy> Buddies { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationMember> ConversationMembers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageReaction> MessageReactions { get; set; }
        public DbSet<ExternalLogin> ExternalLogins { get; set; }
        public DbSet<BookRating> BookRatings { get; set; }
        public DbSet<BookReview> BookReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserAuthIdentity>()
                    .HasKey(x => x.Id);
            modelBuilder.Entity<UserAuthIdentity>()
                    .HasOne(x => x.User)
                    .WithMany(x => x.AuthIdentities)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserAuthIdentity>()
                    .HasIndex(x => new { x.Provider, x.ProviderKey })
                    .IsUnique();
            modelBuilder.Entity<User>()
                    .HasIndex(x => x.Email)
                    .IsUnique();
            modelBuilder.Entity<User>()
                    .HasIndex(x => x.UserName)
                    .IsUnique();



            modelBuilder.Entity<UserBook>()
                .HasKey(ub => new { ub.UserId, ub.BookId }); // composite PK
            modelBuilder.Entity<UserBook>()
                .HasOne(ub => ub.User)
                .WithMany(u => u.WantReadJoin)
                .HasForeignKey(ub => ub.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<UserBook>()
                .HasOne(ub => ub.Book)
                .WithMany(b => b.UserBookJoin)
                .HasForeignKey(ub => ub.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BuddyRequest>()
                .HasKey(brj => new { brj.ActiveUserId, brj.PassiveUserId }); // composite PK
            modelBuilder.Entity<BuddyRequest>()
                .HasOne(brj => brj.ActiveUser)
                .WithMany(u => u.SentBuddyRequestsJoin)
                .HasForeignKey(brj => brj.ActiveUserId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<BuddyRequest>()
                .HasOne(brj => brj.PassiveUser)
                .WithMany(u => u.ReceivedBuddyRequestsJoin)
                .HasForeignKey(brj => brj.PassiveUserId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Buddy>()
                .HasKey(b => new { b.UserAId, b.UserBId });
            modelBuilder.Entity<Buddy>()
                .HasOne(b => b.UserA)
                .WithMany()
                .HasForeignKey(b => b.UserAId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Buddy>()
                .HasOne(b => b.UserB)
                .WithMany()
                .HasForeignKey(b => b.UserBId)
                .OnDelete(DeleteBehavior.NoAction);

            // Composite key for ConversationMember (many-to-many)
            modelBuilder.Entity<ConversationMember>()
                .HasKey(cm => new { cm.UserId, cm.ConversationId });

            modelBuilder.Entity<ConversationMember>()
                .HasOne(cm => cm.User)
                .WithMany(u => u.ConversationMembers)
                .HasForeignKey(cm => cm.UserId);

            modelBuilder.Entity<ConversationMember>()
                .HasOne(cm => cm.Conversation)
                .WithMany(c => c.Members)
                .HasForeignKey(cm => cm.ConversationId);

            // One-to-many for Messages
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.SenderId);

            // One-to-many for MessageReactions
            modelBuilder.Entity<MessageReaction>()
                .HasOne(r => r.Message)
                .WithMany(m => m.Reactions)
                .HasForeignKey(r => r.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MessageReaction>()
                .HasOne(r => r.User)
                .WithMany(u => u.MessageReactions)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Conversation>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.Conversation)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<BookRating>(entity =>
            {
                // Primary key
                entity.HasKey(br => br.Id);

                // User foreign key
                entity.HasOne(br => br.User)
                    .WithMany(u => u.BookRatings)
                    .HasForeignKey(br => br.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Book foreign key
                entity.HasOne(br => br.Book)
                    .WithMany()
                    .HasForeignKey(br => br.BookId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Rating validation at the database level
                entity.Property(br => br.Rating)
                    .IsRequired();

                // Prevent duplicate ratings by the same user for the same book
                entity.HasIndex(br => new { br.UserId, br.BookId })
                    .IsUnique();
            });
            modelBuilder.Entity<BookReview>(entity =>
            {
                // Primary key
                entity.HasKey(br => br.Id);

                // User foreign key
                entity.HasOne(br => br.User)
                    .WithMany(u => u.BookReviews)
                    .HasForeignKey(br => br.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Book foreign key
                entity.HasOne(br => br.Book)
                    .WithMany()
                    .HasForeignKey(br => br.BookId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Review validation at the database level
                entity.Property(br => br.Review)
                    .IsRequired()
                    .HasMaxLength(1000);

                // Prevent duplicate reviews by the same user for the same book
                entity.HasIndex(br => new { br.UserId, br.BookId })
                    .IsUnique();
            });

        }
    }
}
