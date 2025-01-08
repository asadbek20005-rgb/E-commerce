﻿using Ec.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ec.Data.ConfigurationModels;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(500);
        builder.Property(x => x.Role)
            .IsRequired();


        builder.HasOne(x => x.Address)
        .WithOne(x => x.Seller)
        .HasForeignKey<Address>(x => x.SellerId);

        builder.HasOne(x => x.Statistic)
        .WithOne(x => x.Seller)
        .HasForeignKey<Statistic>(x => x.SellerId);

        builder.HasMany(x => x.Products)
        .WithOne(x => x.Seller)
        .HasForeignKey(x => x.SellerId);

        builder.HasMany(x => x.Feedbacks)
        .WithOne(x => x.User)
        .HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.Feedbacks)
        .WithOne(x => x.User)
        .HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.SearchHistories)
        .WithOne(x => x.Client)
        .HasForeignKey(x => x.ClientId);

        builder.HasMany(x => x.Complaints)
        .WithOne(x => x.Client)
        .HasForeignKey(x => x.ClientId);

        builder.HasMany(x => x.Chats)
        .WithOne(x => x.User)
        .HasForeignKey(x => x.UserId);


        builder.HasData(new User
        {
            Id = Guid.NewGuid(),
            FullName = "Shermatov Asadbek",
            PhoneNumber = "+998945631282",
            Username ="spawn",
            IsBlocked = false,
            Role = "super admin",
            
        });
    }
}
