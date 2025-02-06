using Ec.Data.Entities;
using Ec.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ec.Data.ConfigurationModels;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Rank).HasConversion(v => v.ToString(), v => (Rank)Enum.Parse(typeof(Rank), v));
    }
}
