using BookStore.Data;
using BookStore.Models;

namespace TestSetup;
public static class Genres
{
    public static void AddGenres(this AppDbContext context)
    {
        context.Genres.AddRange(
            new Genre { Id = 1, Name = "Personal Growth" , Description = "Books that help you grow personally and professionally", IsActive = true },
            new Genre { Id = 2, Name = "Science Fiction", Description = "Books that are set in a futuristic world", IsActive = true },
            new Genre { Id = 3, Name = "Romance", Description = "Books that focus on romantic relationships", IsActive = true },
            new Genre { Id = 4, Name = "Mystery", Description = "Books that involve solving a mystery", IsActive = true }
        );
        context.SaveChanges();
    }
}
