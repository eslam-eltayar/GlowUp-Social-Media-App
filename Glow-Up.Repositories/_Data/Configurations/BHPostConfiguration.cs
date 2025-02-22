using Glow_Up.Core.Enums;
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
    public class BHPostConfiguration : IEntityTypeConfiguration<BHPost>
    {
        public void Configure(EntityTypeBuilder<BHPost> builder)
        {
            builder.Property(p => p.Caption)
          .HasMaxLength(255);

            builder.Property(c => c.Category)
                     .HasConversion(
                     (type) => type.ToString(),
                     (gen) => (Category)Enum.Parse(typeof(Category), gen, true));
        }
    }
}
