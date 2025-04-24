using TestSetup;
using BookStore.Data;
using FluentAssertions;
using AutoMapper;
using BookStore.Models;
using BookStore.Application.AuthorOperations.Command.CreateAuthor;

namespace Tests.BookStore.UnitTests.Application.AuthorOperations.Command.CreateAuthor;

public class CreateAuthorCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CreateAuthorCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void WhenAlreadyExistAuthorTitleIsGiven_InvalidOperationException_ShouldBeReturn()
    {
        // Arrange (Hazırlık)
        var Author = new Author()
        {
            FirstName = "Test_WhenAlreadyExistAuthorTitleIsGiven_InvalidOperationException_ShouldBeReturn",
            LastName = "Test_WhenAlreadyExistAuthorTitleIsGiven_InvalidOperationException_ShouldBeReturn",
            BirthDate = new DateTime(1990, 1, 1),
            IsActive = true,
        };
        _context.Authors.Add(Author);
        _context.SaveChanges();

        CreateAuthorCommand command = new CreateAuthorCommand(_context, _mapper);
        command.Model = new CreateAuthorModel() { FirstName = Author.FirstName, LastName = Author.LastName , BirthDate = Author.BirthDate };

        // Act & Assert (Çalıştırma - Doğrulama)
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And.Message.Should().Be("Author already exists.");
    }

    [Fact]
    public void WhenValidInputsAreGiven_Author_ShouldBeCreated()
    {
        //arrange
        CreateAuthorCommand command = new CreateAuthorCommand(_context, _mapper);
        CreateAuthorModel model = new CreateAuthorModel() { FirstName = "Author111", LastName = "Author222", BirthDate = new DateTime(1990, 1, 1)};
        command.Model = model;

        //act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        //assert
        var Author = _context.Authors.SingleOrDefault(Author => Author.FirstName == model.FirstName && Author.LastName == model.LastName && Author.BirthDate == model.BirthDate);
        Author.Should().NotBeNull();
        Author.FirstName.Should().Be(model.FirstName);
        Author.LastName.Should().Be(model.LastName);
        Author.BirthDate.Should().Be(model.BirthDate);
        Author.IsActive.Should().Be(true);
    }
}
