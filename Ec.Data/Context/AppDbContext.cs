using Ec.Data.ConfigurationModels;
using Ec.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualBasic;
namespace Ec.Data.Context;

public class AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<User> Users { get; set; }
    public DbSet<User_Chat> User_Chats { get; set; }
    public DbSet<Statistic> Statistics { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<OTP> OTPs { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Complaint> Complaints { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Address> Address { get; set; }
    public DbSet<SearchHistory> SearchHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ChatConfiguration());
        modelBuilder.ApplyConfiguration(new ComplaintConfiguration());
       

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.ConfigureWarnings(warnings =>
        {
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning);
        });

     
    }
}