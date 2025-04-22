using BookStore.Data;

namespace BookStore.Application.BookOperations.Command.DeleteBook;

public class DeleteBookCommand
{
    private readonly AppDbContext _context;
    public int BookId { get; set; }

    public DeleteBookCommand(AppDbContext context)
    {
        _context = context;
    }

    public void Handle()
    {
        var book = _context.Books.SingleOrDefault(x => x.Id == BookId);
        if (book == null)
            throw new InvalidOperationException("Book not found.");

        if (book.IsActive == false)
            throw new InvalidOperationException("Book is already inactive.");
        book.IsActive = false; 
        _context.SaveChanges();
    }
}
