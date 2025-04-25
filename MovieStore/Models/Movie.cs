using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieStore.Models;


  
public class Movie : BaseModel
{
    [Required]
    public string Title { get; set; }

    public int ReleaseYear { get; set; }

    public GenreEnum Genre { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    public int DirectorId { get; set; }

    [ForeignKey("DirectorId")]
    public Director Director { get; set; }

    public List<Actor> Actors { get; set; }
    public List<Order> Orders { get; set; }
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
       
        
    }
}
