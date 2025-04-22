using BookStore.Data;
using AutoMapper;

namespace BookStore.Application.BookOperations.Query.GetBooks;

public class GetBooksQuery
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetBooksQuery(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public List<BooksViewModel> Handle()
    {
        var bookList = _dbContext.Books.OrderBy(x => x.Id).ToList();
        var result = _mapper.Map<List<BooksViewModel>>(bookList); // Use AutoMapper to map Book to BooksViewModel
        return result;
    }

}

public class BooksViewModel
{
    public string Title { get; set; }
    public string PublishedDate { get; set; }
    public string Genre { get; set; }

}

