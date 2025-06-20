using Glow_Up.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Repositories._Data.Configurations;
public class ReportPostConfiguration : IEntityTypeConfiguration<ReportPost>
{
    public void Configure(EntityTypeBuilder<ReportPost> builder)
    {
        builder.HasOne(rp => rp.Post)
            .WithMany()
            .HasForeignKey(rp => rp.PostId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
