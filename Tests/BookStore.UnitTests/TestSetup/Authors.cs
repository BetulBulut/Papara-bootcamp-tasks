using BookStore.Data;
using BookStore.Models;

namespace TestSetup;
public static class Authors
{
    public static void AddAuthors(this AppDbContext context)
    {
        context.Authors.AddRange(
            new Author { Id = 1, FirstName = "Eric Ries", LastName = "Ries", BirthDate = new DateTime(1980, 09, 22), IsActive = true },
            new Author { Id = 2, FirstName = "Charlotte Perkins Gilman", LastName = "Gilman", BirthDate = new DateTime(1860, 07, 03), IsActive = true },
            new Author { Id = 3, FirstName = "Frank Herbert", LastName = "Herbert", BirthDate = new DateTime(1920, 10, 08), IsActive = true }
        );
        context.SaveChanges();
    }
}
