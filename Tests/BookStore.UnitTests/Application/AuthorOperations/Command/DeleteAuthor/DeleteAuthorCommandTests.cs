using TestSetup;
using BookStore.Data;
using FluentAssertions;
using BookStore.Application.AuthorOperations.Command;
using BookStore.Models;

namespace Tests.BookStore.UnitTests.Application.AuthorOperations.Command.DeleteAuthor;

public class DeleteAuthorCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly AppDbContext _context;

    public DeleteAuthorCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
    }

    [Fact]
    public void WhenAuthorIsNotFound_InvalidOperationException_ShouldBeThrown()
    {
        // Arrange
        DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
        command.AuthorId = 999; // Non-existent Author ID
        
        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And.Message.Should().Be("Author not found.");
    }

    [Fact]
    public void WhenAuthorIsFound_AuthorShouldBeSoftDeletedSuccessfully()
    {
        // Arrange
        var Author = new Author()
        {
            FirstName = "Test",
            LastName = "Author",
            IsActive = true
        };
        _context.Authors.Add(Author);
        _context.SaveChanges();

        DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
        command.AuthorId = Author.Id;

        // Act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // Assert
        var softDeletedAuthor = _context.Authors.SingleOrDefault(b => b.Id == Author.Id);
        softDeletedAuthor.Should().NotBeNull();
        softDeletedAuthor!.IsActive.Should().BeFalse();
    }
}
