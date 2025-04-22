using BookStore.Data;
using AutoMapper;

namespace BookStore.Application.GenreOperations.Command;

public class UpdateGenreCommand
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public int GenreId { get; set; }
    public UpdateGenreModel UpdatedGenre { get; set; }

    public UpdateGenreCommand(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public void Handle()
    {
        var Genre = _context.Genres.SingleOrDefault(x => x.Id == GenreId);
        if (Genre == null)
            throw new InvalidOperationException("Genre not found.");

        _mapper.Map(UpdatedGenre, Genre);
        _context.SaveChanges();
    }
}

public class UpdateGenreModel
{
    public string Name { get; set; }
    public string Description { get; set; }
}
