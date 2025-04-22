using BookStore.Data;
using AutoMapper;

namespace BookStore.Application.GenreOperations.Query;

public class GetGenresQuery
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetGenresQuery(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public List<GenresViewModel> Handle()
    {
        var GenreList = _dbContext.Genres.OrderBy(x => x.Id).ToList();
        var result = _mapper.Map<List<GenresViewModel>>(GenreList);
        return result;
    }

}

public class GenresViewModel
{
    public string Name { get; set; }
    public string Description { get; set; }

}

