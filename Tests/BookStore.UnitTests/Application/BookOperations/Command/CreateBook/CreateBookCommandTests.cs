using TestSetup;
using BookStore.Data;
using FluentAssertions;
using AutoMapper;
using BookStore.Application.BookOperations.Command.CreateBook;
using BookStore.Models;

namespace Tests.BookStore.UnitTests.Application.BookOperations.Command.CreateBook;

public class CreateBookCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CreateBookCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void WhenAlreadyExistBookTitleIsGiven_InvalidOperationException_ShouldBeReturn()
    {
        // Arrange (Hazırlık)
        var book = new Book()
        {
            Title = "Test_WhenAlreadyExistBookTitleIsGiven_InvalidOperationException_ShouldBeReturn",
            PublishedDate = new System.DateTime(1990, 01, 10),
            GenreId = 1,
            AuthorId = 1,
            IsActive = true,
            Price = 10,
            ISBN = "1234567890"
        };
        _context.Books.Add(book);
        _context.SaveChanges();

        CreateBookCommand command = new CreateBookCommand(_context, _mapper);
        command.Model = new CreateBookModel() { Title = book.Title };

        // Act & Assert (Çalıştırma - Doğrulama)
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And.Message.Should().Be("Book already exists.");
    }

    [Fact]
    public void WhenValidInputsAreGiven_Book_ShouldBeCreated()
    {
        //arrange
        CreateBookCommand command = new CreateBookCommand(_context, _mapper);
        CreateBookModel model = new CreateBookModel() { Title = "book111", PublishedDate = DateTime.Now.Date.AddYears(-2), GenreId = 1, Price = 10, ISBN = "1234567890", AuthorId =1 };
        command.Model = model;

        //act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        //assert
        var book = _context.Books.SingleOrDefault(book => book.Title == model.Title);
        book.Should().NotBeNull();
        book.PublishedDate.Should().Be(model.PublishedDate);
        book.GenreId.Should().Be(model.GenreId);
        book.Price.Should().Be(model.Price);
        book.ISBN.Should().Be(model.ISBN);
        book.IsActive.Should().Be(true);
    }
}
