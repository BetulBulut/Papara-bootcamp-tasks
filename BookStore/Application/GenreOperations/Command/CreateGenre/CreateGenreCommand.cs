using BookStore.Data;
using BookStore.Models;
using AutoMapper;

namespace BookStore.Application.GenreOperations.Command;

public class CreateGenreCommand
{
    public CreateGenreModel Model { get; set; }
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateGenreCommand(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public void Handle()
    {
        var Genre = _dbContext.Genres.SingleOrDefault(b => b.Name == Model.Name);
        if (Genre is not null)
            throw new InvalidOperationException("Genre already exists.");

        Genre = _mapper.Map<Genre>(Model);
        Genre.IsActive = true; 
        _dbContext.Genres.Add(Genre);
        _dbContext.SaveChanges();
    }
}

public class CreateGenreModel
{
    public string Description { get; set; }  
    public string Name { get; set; }  
}
