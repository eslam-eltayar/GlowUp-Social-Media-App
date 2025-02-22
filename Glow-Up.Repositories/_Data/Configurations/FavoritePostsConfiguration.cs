using Glow_Up.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Repositories._Data.Configurations
{
    internal class FavoritePostsConfiguration : IEntityTypeConfiguration<FavoritePost>
    {
        public void Configure(EntityTypeBuilder<FavoritePost> builder)
        {

            builder.HasOne(fp => fp.User)
                .WithMany()
                .HasForeignKey(fp => fp.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(fp => fp.Post)
                .WithMany()
                .HasForeignKey(fp => fp.PostId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
