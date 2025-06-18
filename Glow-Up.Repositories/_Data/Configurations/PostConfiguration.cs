using Glow_Up.Core.Enums;
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
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(p => p.Caption)
                .HasMaxLength(255);

            builder.Property(p => p.PostType)
                .HasConversion(
                    v => v.ToString(),
                    v => (PostType)Enum.Parse(typeof(PostType), v));

            builder.HasMany(x => x.Reactions)
                .WithOne(x => x.Post)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
