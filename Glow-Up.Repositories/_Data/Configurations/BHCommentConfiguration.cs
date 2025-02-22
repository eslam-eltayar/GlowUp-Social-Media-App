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
    public class BHCommentConfiguration : IEntityTypeConfiguration<BHComment>
    {
        public void Configure(EntityTypeBuilder<BHComment> builder)
        {
            builder.HasOne(c => c.User)
                 .WithMany(u => u.BHComment)
                 .HasForeignKey(c => c.UserId)
                 .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
