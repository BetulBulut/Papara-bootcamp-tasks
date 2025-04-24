using BookStore.Data;
using AutoMapper;

namespace BookStore.Application.BookOperations.Command.UpdateBook;

public class UpdateBookCommand
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public int BookId { get; set; }
    public UpdateBookModel UpdatedBook { get; set; }

    public UpdateBookCommand(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public void Handle()
    {
        var book = _context.Books.SingleOrDefault(x => x.Id == BookId);
        if (book == null)
            throw new InvalidOperationException("Book not found.");

        _mapper.Map(UpdatedBook, book); // Use AutoMapper to map UpdatedBook to the existing Book entity
        _context.SaveChanges();
    }
}

public class UpdateBookModel
{
    public string Title { get; set; }
    public int GenreId { get; set; }
    public int Price { get; set; }
    public int AuthorId { get; set; }
}
