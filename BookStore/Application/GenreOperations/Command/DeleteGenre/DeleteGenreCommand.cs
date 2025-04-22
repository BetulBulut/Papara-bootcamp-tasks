using BookStore.Data;

namespace BookStore.Application.GenreOperations.Command;

public class DeleteGenreCommand
{
    private readonly AppDbContext _context;
    public int GenreId { get; set; }

    public DeleteGenreCommand(AppDbContext context)
    {
        _context = context;
    }

    public void Handle()
    {
        var genre = _context.Genres.SingleOrDefault(x => x.Id == GenreId);
        if (genre == null)
            throw new InvalidOperationException("Genre not found.");

        if (genre.IsActive == false)
            throw new InvalidOperationException("Genre is already inactive.");
        genre.IsActive = false; 
        _context.SaveChanges();
    }
}
