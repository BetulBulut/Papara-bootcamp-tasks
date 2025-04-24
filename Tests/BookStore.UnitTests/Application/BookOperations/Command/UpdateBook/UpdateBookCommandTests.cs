using TestSetup;
using BookStore.Data;
using FluentAssertions;
using AutoMapper;
using BookStore.Application.BookOperations.Command.UpdateBook;
using BookStore.Models;

namespace Tests.BookStore.UnitTests.Application.BookOperations.Command.UpdateBook;

public class UpdateBookCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UpdateBookCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void WhenBookIsNotFound_InvalidOperationException_ShouldBeThrown()
    {
        // Arrange
        UpdateBookCommand command = new UpdateBookCommand(_context, _mapper);
        command.BookId = 999; // Non-existent book ID
        command.UpdatedBook = new UpdateBookModel() { Title = "NonExistentBook", GenreId = 1, Price = 10, AuthorId = 1 };

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And.Message.Should().Be("Book not found.");
    }

    [Fact]
    public void WhenBookIsFound_BookShouldBeUpdatedSuccessfully()
    {
        // Arrange
        var book = new Book()
        {
            Title = "Original Title",
            PublishedDate = new System.DateTime(2000, 01, 01),
            GenreId = 1,
            AuthorId = 1,
            IsActive = true,
            Price = 20,
            ISBN = "1234567890"
        };
        _context.Books.Add(book);
        _context.SaveChanges();

        UpdateBookCommand command = new UpdateBookCommand(_context, _mapper);
        command.BookId = book.Id;
        command.UpdatedBook = new UpdateBookModel()
        {
            Title = "Updated Title",
            GenreId = 2,
            Price = 25,
            AuthorId = 2
        };

        // Act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // Assert
        var updatedBook = _context.Books.SingleOrDefault(b => b.Id == book.Id);
        updatedBook.Should().NotBeNull();
        updatedBook.Title.Should().Be("Updated Title");
        updatedBook.GenreId.Should().Be(2);
        updatedBook.Price.Should().Be(25);
        updatedBook.AuthorId.Should().Be(2);
    }
}
