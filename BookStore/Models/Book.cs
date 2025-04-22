using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public DateTime PublishedDate { get; set; }
    public decimal Price { get; set; }

    public int GenreId { get; set; }
}

public class BookConfiguration : IEntityTypeConfiguration<Book>
{

    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.Property(b => b.Id).ValueGeneratedOnAdd()
            .ValueGeneratedOnAdd();
        builder.Property(b => b.Title).IsRequired(true).HasMaxLength(100);
        builder.Property(b => b.Author).IsRequired(true).HasMaxLength(100);
        builder.Property(b => b.ISBN).IsRequired(true).HasMaxLength(50);
        builder.Property(b => b.PublishedDate).IsRequired(true);
        builder.Property(b => b.Price).IsRequired(true);
        builder.Property(b => b.GenreId).IsRequired(true);

    }
}
