using Glow_Up.Core.Enums;
using Glow_Up.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Repositories._Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);

            builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);

            builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);


            builder.Property(c => c.Gender)
              .HasConversion(
              (gender) => gender.ToString(),
              (gen) => (Gender)Enum.Parse(typeof(Gender), gen, true));

            // Relationships
            //builder.HasMany(u => u.Posts)
            //    .WithOne(p => p.User)
            //    .HasForeignKey(p => p.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(u => u.Comments)
            //    .WithOne(c => c.User)
            //    .HasForeignKey(c => c.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Reactions)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
