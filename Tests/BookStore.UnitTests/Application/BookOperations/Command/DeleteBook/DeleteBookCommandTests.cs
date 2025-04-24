using TestSetup;
using BookStore.Data;
using FluentAssertions;
using BookStore.Application.BookOperations.Command.DeleteBook;
using BookStore.Models;

namespace Tests.BookStore.UnitTests.Application.BookOperations.Command.DeleteBook;

public class DeleteBookCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly AppDbContext _context;

    public DeleteBookCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
    }

     [Fact]
    public void WhenBookIsNotFound_InvalidOperationException_ShouldBeThrown()
    {
        // Arrange
        DeleteBookCommand command = new DeleteBookCommand(_context);
        command.BookId = 999; // Non-existent book ID
        
        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And.Message.Should().Be("Book not found.");
    }

    [Fact]
    public void WhenBookIsFound_BookShouldBeSoftDeletedSuccessfully()
    {
        // Arrange
        var book = new Book()
        {
            Title = "Test Book",
            PublishedDate = new System.DateTime(2000, 01, 01),
            GenreId = 1,
            AuthorId = 1,
            IsActive = true,
            Price = 20,
            ISBN = "1234567890"
        };
        _context.Books.Add(book);
        _context.SaveChanges();

        DeleteBookCommand command = new DeleteBookCommand(_context);
        command.BookId = book.Id;

        // Act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // Assert
        var softDeletedBook = _context.Books.SingleOrDefault(b => b.Id == book.Id);
        softDeletedBook.Should().NotBeNull();
        softDeletedBook!.IsActive.Should().BeFalse();
    }
}
