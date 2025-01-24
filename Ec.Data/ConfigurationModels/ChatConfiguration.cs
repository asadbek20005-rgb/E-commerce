using Ec.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ec.Data.ConfigurationModels;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.ToTable("Chats");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Names).IsRequired();
        builder.HasMany(x => x.Users)
            .WithOne(x => x.Chat)
            .HasForeignKey(x => x.ChatId);
        builder.HasMany(x => x.Messages)
            .WithOne(x => x.Chat)
            .HasForeignKey(x => x.ChatId);

    }
}
