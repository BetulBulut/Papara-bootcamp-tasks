using BookStore.Data;

namespace BookStore.BookOperations;

public class UpdateBookCommand
{
    private readonly AppDbContext _context;
    public int BookId { get; set; }
    public UpdateBookModel UpdatedBook { get; set; }

    public UpdateBookCommand(AppDbContext context)
    {
        _context = context;
    }

    public void Handle()
    {
        var book = _context.Books.SingleOrDefault(x => x.Id == BookId);
        if (book == null)
            throw new InvalidOperationException("Book not found.");

        book.GenreId = UpdatedBook.GenreId != default ? UpdatedBook.GenreId : book.GenreId;
        book.Title = UpdatedBook.Title != default ? UpdatedBook.Title : book.Title;
        book.Price = UpdatedBook.Price != default ? UpdatedBook.Price : book.Price;

        _context.SaveChanges();
    }
}

public class UpdateBookModel
{
    public string Title { get; set; }
    public int GenreId { get; set; }
    public int Price { get; set; }
}
