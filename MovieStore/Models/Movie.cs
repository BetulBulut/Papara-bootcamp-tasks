using Base.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieStore.Models;

public class Movie : BaseModel
{
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
    public GenreEnum Genre { get; set; } 
    public decimal Price { get; set; }
    public int DirectorId { get; set; }
    public Director Director { get; set; }
    public List<Actor> Actors { get; set; } 

}

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.Property(b => b.Id).ValueGeneratedOnAdd();
        builder.Property(b => b.Title).IsRequired(true).HasMaxLength(100);
        builder.Property(b => b.ReleaseYear).IsRequired(true);
        builder.Property(b => b.Price).IsRequired(true).HasColumnType("decimal(18,2)");
        builder.Property(b => b.Genre).IsRequired(true);
       
        builder.Property(b => b.DirectorId).IsRequired(true);
        builder.HasOne(b => b.Director)
            .WithMany(b => b.DirectedMovies)
            .HasForeignKey(b => b.DirectorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        
    }
}
