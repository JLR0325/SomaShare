using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SomaShare.Models;

namespace SomaShare.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Textbook> Textbooks { get; set; }
        public DbSet<WantedAd> WantedAds { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ForumThread> ForumThreads { get; set; }
        public DbSet<ForumPost> ForumPosts { get; set; }
        public DbSet<Models.ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Textbook --> User
            builder.Entity<Textbook>()
                .HasOne(t => t.User)
                .WithMany(u => u.Textbooks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Offer --> Textbook (cascade) and Offer -> User (restrict)
            builder.Entity<Offer>()
                .HasOne(o => o.Textbook)
                .WithMany(t => t.Offers)
                .HasForeignKey(o => o.TextbookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Offer>()
                .HasOne(o => o.User)
                .WithMany(u => u.OffersMade)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Transaction one-to-one with Offer
            builder.Entity<Transaction>()
                .HasOne(t => t.Offer)
                .WithOne(o => o.Transaction)
                .HasForeignKey<Transaction>(t => t.OfferId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Transaction>()
                .HasOne(t => t.Buyer)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Transaction>()
                .HasOne(t => t.Seller)
                .WithMany()
                .HasForeignKey(t => t.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Review: many-to-many between users (Reviewer and Reviewed)
            builder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany(u => u.ReviewsWritten)
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(r => r.ReviewedUser)
                .WithMany(u => u.ReviewsReceived)
                .HasForeignKey(r => r.ReviewedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Forum
            builder.Entity<ForumThread>()
                .HasMany(t => t.Posts)
                .WithOne(p => p.Thread)
                .HasForeignKey(p => p.ThreadId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ForumPost>()
                .HasOne(p => p.User)
                .WithMany(u => u.ForumPosts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes for faster searching
            builder.Entity<Textbook>()
                .HasIndex(t => t.Title);
            builder.Entity<Textbook>()
                .HasIndex(t => t.Author);
            builder.Entity<Textbook>()
                .HasIndex(t => t.ISBN);
            builder.Entity<Textbook>()
                .HasIndex(t => t.Campus);
            builder.Entity<Textbook>()
                .HasIndex(t => t.Price);

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.FullName);
        }
    }
}
