using System;
using Base.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieStore.Models;

public class Order : BaseModel
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PriceAtPurchase { get; set; }
}

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(b => b.Id).ValueGeneratedOnAdd();
        builder.Property(b => b.CustomerId).IsRequired(true);
        builder.Property(b => b.MovieId).IsRequired(true);
        builder.Property(b => b.PurchaseDate).IsRequired(true);
        builder.Property(b => b.PriceAtPurchase).IsRequired(true).HasColumnType("decimal(18,2)");

        builder.HasOne(b => b.Customer).WithMany(b => b.Orders).HasForeignKey(b => b.CustomerId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(b => b.Movie).WithMany().HasForeignKey(b => b.MovieId).OnDelete(DeleteBehavior.NoAction);
    }
}
