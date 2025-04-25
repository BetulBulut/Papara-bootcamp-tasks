using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Base.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieStore.Models;

public class Actor : BaseModel
{
    [Required, MaxLength(50)]
    public string FirstName { get; set; }

    [Required, MaxLength(50)]
    public string LastName { get; set; }

    public List<Movie> Movies { get; set; }
}
public class ActorConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.Property(b => b.Id).ValueGeneratedOnAdd();
        builder.Property(b => b.FirstName).IsRequired(true).HasMaxLength(50);
        builder.Property(b => b.LastName).IsRequired(true).HasMaxLength(50);
       
    }
}
