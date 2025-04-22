using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Models;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; } = true;
    public List<Book> Books { get; set; } = new List<Book>();
}
public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{

    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.Property(g => g.Id).ValueGeneratedOnAdd()
            .ValueGeneratedOnAdd();
        builder.Property(g => g.Name).IsRequired(true).HasMaxLength(50);
        builder.Property(g => g.Description).IsRequired(false).HasMaxLength(200);
        builder.Property(b => b.IsActive).IsRequired(true).HasDefaultValue(true);
    }
}

