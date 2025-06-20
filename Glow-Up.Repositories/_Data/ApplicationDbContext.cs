using Glow_Up.Core.Models;
using Glow_Up.Core.Models.BlackHat;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Repositories._Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Recipient)
                .WithMany()
                .HasForeignKey(m => m.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<FavoritePost> FavoritePosts { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<ReportPost> ReportPosts { get; set; }


        // BlackHat 
        public DbSet<BHPost> BHPosts { get; set; }
        public DbSet<BHComment> BHComments { get; set; }
        public DbSet<BHMedia> BHMedias { get; set; }
        public DbSet<BHLike> BHLikes { get; set; }
    }
}
