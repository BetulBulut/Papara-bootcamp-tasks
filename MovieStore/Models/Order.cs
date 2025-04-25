using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieStore.Models;

public class Order : BaseModel
{
  [Required]
    public int CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; }

    [Required]
    public int MovieId { get; set; }

    [ForeignKey("MovieId")]
    public Movie Movie { get; set; }

    [Required]
    public DateTime PurchaseDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PriceAtPurchase { get; set; }
}

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        
        builder.Property(b => b.Id)
               .IsRequired()
               .HasColumnName("Id")
               .HasColumnType("int")
               .ValueGeneratedOnAdd();

        builder.Property(b => b.CustomerId).IsRequired();
        builder.Property(b => b.MovieId).IsRequired();
        builder.Property(b => b.PurchaseDate).IsRequired();
        builder.Property(b => b.PriceAtPurchase)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        // Foreign Key ilişkileri burada açıkça belirtiliyor:
        builder.HasOne(o => o.Customer)
               .WithMany(c => c.Orders)
               .HasForeignKey(o => o.CustomerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Movie)
               .WithMany(m=>m.Orders)
               .HasForeignKey(o => o.MovieId)
               .OnDelete(DeleteBehavior.Restrict);
        
    }
}
