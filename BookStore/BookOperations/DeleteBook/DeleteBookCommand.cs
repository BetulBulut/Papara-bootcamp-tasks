using System;
using System.Linq;
using BookStore.Data;

namespace BookStore.BookOperations;

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

        _context.Books.Remove(book);
        _context.SaveChanges();
    }
}
