using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BookStore.Data;
using BookStore.Models;
using System.Collections.Generic;
using BookStore.Common;

namespace BookStore.BookOperations;

public class GetBooksQuery
{
    private readonly AppDbContext _dbContext;

    public GetBooksQuery(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<BooksViewModel> Handle()
    {
        var bookList = _dbContext.Books.OrderBy(x => x.Id).ToList();
        List<BooksViewModel> vm = new List<BooksViewModel>();

        foreach (var book in bookList)
        {
            vm.Add(new BooksViewModel()
            {
                Title = book.Title,
                PublishedDate = book.PublishedDate.Date.ToString("dd/MM/yyyy"),
                Genre = ((GenreEnum)book.GenreId).ToString(),
            });
        }
        return vm;
    }

}

public class BooksViewModel
{
    public string Title { get; set; }
    public string PublishedDate { get; set; }
    public string Genre { get; set; }

}

