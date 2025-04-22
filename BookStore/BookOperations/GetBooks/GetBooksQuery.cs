using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BookStore.Data;
using BookStore.Models;
using System.Collections.Generic;

namespace BookStore.BookOperations;

public class GetBooksQuery
{
    private readonly AppDbContext _dbContext;

    public GetBooksQuery(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Book> Handle()
    {
        var bookList = _dbContext.Books.OrderBy(x => x.Id).ToList<Book>();
        return bookList;
    }
}

public class BooksViewModel
{
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime PublishedDate { get; set; }
    public decimal Price { get; set; }
}

