using Base.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieStore.Models;

public class Customer : BaseModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<GenreEnum> FavoriteGenres { get; set; } 
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Secret { get; set; }
    public List<Order> Orders { get; set; }
}

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(b => b.Id).ValueGeneratedOnAdd();
        builder.Property(b => b.FirstName).IsRequired(true).HasMaxLength(50);
        builder.Property(b => b.LastName).IsRequired(true).HasMaxLength(50);
        builder.Property(b => b.Username).IsRequired(true).HasMaxLength(50);
        builder.HasIndex(b => b.Username).IsUnique();
        builder.Property(b => b.PasswordHash).IsRequired(true).HasMaxLength(100);
    }
}
