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
    public class BHLikeConfiguration : IEntityTypeConfiguration<BHLike>
    {
        public void Configure(EntityTypeBuilder<BHLike> builder)
        {
            builder.HasOne(r => r.BHPost)
                 .WithMany(p => p.Likes)
                 .HasForeignKey(r => r.BHPostId)
                 .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
