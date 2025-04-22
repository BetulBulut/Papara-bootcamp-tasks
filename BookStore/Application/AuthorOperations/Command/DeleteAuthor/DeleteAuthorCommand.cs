using BookStore.Data;

namespace BookStore.Application.AuthorOperations.Command;

public class DeleteAuthorCommand
{
    private readonly AppDbContext _context;
    public int AuthorId { get; set; }

    public DeleteAuthorCommand(AppDbContext context)
    {
        _context = context;
    }

    public void Handle()
    {
        var author = _context.Authors.SingleOrDefault(x => x.Id == AuthorId);
        if (author == null)
            throw new InvalidOperationException("Author not found.");

        var hasBooks = _context.Books.Any(b => b.AuthorId == AuthorId);
        if (hasBooks)
            throw new InvalidOperationException("Author cannot be deleted because they have published books. Please delete the books first.");

        if (author.IsActive == false)
            throw new InvalidOperationException("Author is already inactive.");

        author.IsActive = false; 
        _context.SaveChanges();
    }
}
