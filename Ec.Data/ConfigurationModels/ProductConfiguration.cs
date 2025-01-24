using Ec.Data.Entities;
using Ec.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ec.Data.ConfigurationModels;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(x => x.Id);  
        builder.Property(x => x.Name).IsRequired().HasMaxLength(500);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(500);
       

        builder.HasMany(x => x.Feedbacks)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.Property(x => x.Category).HasConversion(v => v.ToString(), v => (Category)Enum.Parse(typeof(Category), v));
    }
}
