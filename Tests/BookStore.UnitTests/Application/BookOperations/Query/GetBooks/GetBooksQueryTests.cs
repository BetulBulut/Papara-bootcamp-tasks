using TestSetup;
using BookStore.Data;
using FluentAssertions;
using AutoMapper;
using BookStore.Models;
using BookStore.Application.BookOperations.Query.GetBooks;

namespace Tests.BookStore.UnitTests.Application.BookOperations.Query.GetBooks;

public class GetBooksQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetBooksQueryTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void WhenBooksExist_BooksShouldBeRetrievedSuccessfully()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book { Title = "Book 1", GenreId = 1, AuthorId = 1, PublishedDate = new DateTime(2000, 01, 01), Price = 10, ISBN = "1111111111", IsActive = true },
            new Book { Title = "Book 2", GenreId = 2, AuthorId = 2, PublishedDate = new DateTime(2005, 05, 05), Price = 20, ISBN = "2222222222", IsActive = true }
        };
        _context.Books.AddRange(books);
        _context.SaveChanges();

        GetBooksQuery query = new GetBooksQuery(_context, _mapper);

        // Act
        var result = query.Handle();

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().BeGreaterThanOrEqualTo(2);
        result.Should().Contain(b => b.Title == "Book 1");
        result.Should().Contain(b => b.Title == "Book 2");
    }
 
}
