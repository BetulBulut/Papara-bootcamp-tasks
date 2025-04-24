using TestSetup;
using BookStore.Data;
using FluentAssertions;
using AutoMapper;
using BookStore.Application.AuthorOperations.Command;
using BookStore.Models;

namespace Tests.BookStore.UnitTests.Application.AuthorOperations.Command.UpdateAuthor;

public class UpdateAuthorCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UpdateAuthorCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void WhenAuthorIsNotFound_InvalidOperationException_ShouldBeThrown()
    {
        // Arrange
        UpdateAuthorCommand command = new UpdateAuthorCommand(_context, _mapper);
        command.AuthorId = 999; // Non-existent Author ID
        command.UpdatedAuthor = new UpdateAuthorModel() { FirstName = "NonExistentAuthor", LastName = "Lastname" };

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And.Message.Should().Be("Author not found.");
    }

    [Fact]
    public void WhenAuthorIsFound_AuthorShouldBeUpdatedSuccessfully()
    {
        // Arrange
        var Author = new Author()
        {
            FirstName = "Test Author",
            LastName = "Test Lastname",
            BirthDate = new DateTime(1990, 1, 1),
            IsActive = true
        };
        _context.Authors.Add(Author);
        _context.SaveChanges();

        UpdateAuthorCommand command = new UpdateAuthorCommand(_context, _mapper);
        command.AuthorId = Author.Id;
        command.UpdatedAuthor = new UpdateAuthorModel()
        {
            FirstName = "Updated FirstName",
            LastName = "Updated LastName"
        };

        // Act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // Assert
        var updatedAuthor = _context.Authors.SingleOrDefault(b => b.Id == Author.Id);
        updatedAuthor.Should().NotBeNull();
        updatedAuthor.FirstName.Should().Be("Updated FirstName");
        updatedAuthor.LastName.Should().Be("Updated LastName");
        updatedAuthor.IsActive.Should().Be(true);
    }
}
