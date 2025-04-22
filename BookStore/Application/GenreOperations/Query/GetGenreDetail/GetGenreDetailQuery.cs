using BookStore.Data;
using AutoMapper;
using BookStore.Application.BookOperations.Query.GetBookDetail;

namespace BookStore.Application.GenreOperations.Query;

public class GetGenreDetailQuery
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    public int GenreId { get; set; }

    public GetGenreDetailQuery(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public GenreDetailViewModel Handle()
    {
        var Genre = _dbContext.Genres.SingleOrDefault(Genre => Genre.Id == GenreId);
        if (Genre is null)
            throw new InvalidOperationException("Genre not found.");

        var result = _mapper.Map<GenreDetailViewModel>(Genre); // Use AutoMapper to map Genre to GenreDetailViewModel
        return result;
    }
}

public class GenreDetailViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public List<BookDetailViewModel> Books { get; set; } = new List<BookDetailViewModel>();
    
}
