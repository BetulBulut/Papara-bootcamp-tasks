using BookStore.Data;
using BookStore.Models;

namespace TestSetup;
public static class Books
{
    public static void AddBooks(this AppDbContext context)
    {
        context.Books.AddRange(
            new Book { Id = 1, Title = "Lean Startup", GenreId = 1,AuthorId = 1,  PublishedDate = new DateTime(2001, 06, 12), IsActive = true , Price = 10 ,ISBN = "978-1-234-56789-7" },
            new Book { Id = 2, Title = "Herland", GenreId = 2, AuthorId = 2, PublishedDate = new DateTime(2010, 05, 23) , IsActive = true , Price = 15, ISBN = "978-1-234-56789-8" },
            new Book { Id = 3, Title = "Dune", GenreId = 2, AuthorId = 3, PublishedDate = new DateTime(2001, 12, 21) , IsActive = true , Price = 20 ,ISBN = "978-1-234-56789-9" }
        );
        context.SaveChanges();
    }
}