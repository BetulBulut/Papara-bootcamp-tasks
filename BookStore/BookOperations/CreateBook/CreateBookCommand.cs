
using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookStore.BookOperations;

public class CreateBookCommand
{
    public CreateBookModel Model { get; set; }
    private readonly AppDbContext _dbContext;

    public CreateBookCommand(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Handle()
    {
        var book = _dbContext.Books.SingleOrDefault(b => b.Title == Model.Title);
        if (book is not null)
            throw new InvalidOperationException("Book already exists.");

        book = new Book
        {
            Title = Model.Title,
            GenreId = Model.GenreId,
            PublishedDate = Model.PublishedDate,
            Author = Model.Author,
            ISBN = Model.ISBN,
            Price = Model.Price
        };

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
