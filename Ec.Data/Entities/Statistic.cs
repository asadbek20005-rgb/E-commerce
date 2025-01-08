using System.ComponentModel.DataAnnotations;

namespace Ec.Data.Entities;

public class Statistic
{
    [Key]
    public Guid Id { get; set; }
    public int ProductCount { get; set; }
    public int SoldCount { get; set; }
    public int ViewedCount { get; set; }
    public decimal Earnings { get; set; }

    public Guid SellerId { get; set; }
    public User Seller { get; set; }
}
