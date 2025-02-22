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
    public class MediaConfiguration : IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> builder)
        {
            builder.Property(c => c.Type)
              .HasConversion(
              (type) => type.ToString(),
            (gen) => (MediaType)Enum.Parse(typeof(MediaType), gen, true));

        }
    }
}
