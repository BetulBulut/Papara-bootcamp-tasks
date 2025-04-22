using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Models;

public class Author
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public ICollection<Book> Books { get; set; }
    public bool IsActive { get; set; }
}

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.Property(a => a.Id).ValueGeneratedOnAdd();
        builder.Property(a => a.FirstName).IsRequired(true).HasMaxLength(50);
        builder.Property(a => a.LastName).IsRequired(true).HasMaxLength(50);
        builder.Property(a => a.BirthDate).IsRequired(true);
        builder.Property(a => a.IsActive).IsRequired(true).HasDefaultValue(true);
    }
}