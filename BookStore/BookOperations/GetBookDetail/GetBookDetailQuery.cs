
using BookStore.Common;
using BookStore.Data;

namespace BookStore.BookOperations;

public class GetBookDetailQuery
{
    private readonly AppDbContext _dbContext;
    public int BookId { get; set; }

    public GetBookDetailQuery(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public BookDetailViewModel Handle()
    {
        var book = _dbContext.Books.Where(book => book.Id == BookId).SingleOrDefault();
        if (book is null)
            throw new InvalidOperationException("Book not found.");

        return new BookDetailViewModel
        {
            Title = book.Title,
            Genre = ((GenreEnum)book.GenreId).ToString(),
            PublishedDate = book.PublishedDate.ToString("dd/MM/yyyy"),
            Author = book.Author,
            ISBN = book.ISBN,
            Price = book.Price
        };
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
