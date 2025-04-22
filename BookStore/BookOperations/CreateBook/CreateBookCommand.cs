using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AutoMapper;

namespace BookStore.BookOperations;

public class CreateBookCommand
{
    public CreateBookModel Model { get; set; }
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateBookCommand(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public void Handle()
    {
        var book = _dbContext.Books.SingleOrDefault(b => b.Title == Model.Title);
        if (book is not null)
            throw new InvalidOperationException("Book already exists.");

        book = _mapper.Map<Book>(Model); // Use AutoMapper to map CreateBookModel to Book
        _dbContext.Books.Add(book);
        _dbContext.SaveChanges();
    }
}

public class CreateBookModel
{
    public string Title { get; set; }
    public int GenreId { get; set; }
    public DateTime PublishedDate { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public int Price { get; set; }
}
