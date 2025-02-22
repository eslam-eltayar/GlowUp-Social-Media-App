using Glow_Up.Core.Models.BlackHat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Repositories._Data.Configurations
{
    public class CommentVoteHistoryConfiguration : IEntityTypeConfiguration<CommentVoteHistory>
    {
        public void Configure(EntityTypeBuilder<CommentVoteHistory> builder)
        {
            builder.HasOne(x => x.BHComment)
            .WithMany(c => c.VoteHistory)
            .HasForeignKey(x => x.BHCommentId)
            .OnDelete(DeleteBehavior.NoAction); 


            builder.HasOne(x => x.User)
                .WithMany(u => u.CommentVotes)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.BHCommentId, x.UserId })
                .IsUnique();
        }
    }
}
