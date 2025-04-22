using BookStore.Common;
using BookStore.Data;
using AutoMapper;

namespace BookStore.BookOperations;

public class GetBookDetailQuery
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    public int BookId { get; set; }

    public GetBookDetailQuery(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public BookDetailViewModel Handle()
    {
        var book = _dbContext.Books.SingleOrDefault(book => book.Id == BookId);
        if (book is null)
            throw new InvalidOperationException("Book not found.");

        var result = _mapper.Map<BookDetailViewModel>(book); // Use AutoMapper to map Book to BookDetailViewModel
        return result;
    }
}

public class BookDetailViewModel
{
    public string Title { get; set; }
    public string Genre { get; set; }
    public string PublishedDate { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public decimal Price { get; set;}
}
