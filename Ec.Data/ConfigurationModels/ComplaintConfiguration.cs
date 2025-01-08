using Ec.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ec.Data.ConfigurationModels;

public class ComplaintConfiguration : IEntityTypeConfiguration<Complaint>
{
    public void Configure(EntityTypeBuilder<Complaint> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Text).IsRequired();
        builder.Property(x => x.Status)
            .HasConversion<string>();

    }
}
