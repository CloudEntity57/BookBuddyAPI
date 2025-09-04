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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


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

        }
    }
}
