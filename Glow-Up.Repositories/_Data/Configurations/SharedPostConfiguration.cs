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
    public class SharedPostConfiguration : IEntityTypeConfiguration<SharedPost>
    {
        public void Configure(EntityTypeBuilder<SharedPost> builder)
        {
            builder.HasOne(sp => sp.User)
                   .WithMany()
                   .HasForeignKey(sp => sp.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sp => sp.Post)
                   .WithMany()
                   .HasForeignKey(sp => sp.PostId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
