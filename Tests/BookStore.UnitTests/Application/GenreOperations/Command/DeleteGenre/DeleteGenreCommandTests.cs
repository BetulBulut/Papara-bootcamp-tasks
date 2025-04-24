using TestSetup;
using BookStore.Data;
using FluentAssertions;
using BookStore.Application.GenreOperations.Command;
using BookStore.Models;

namespace Tests.BookStore.UnitTests.Application.GenreOperations.Command.DeleteGenre;

public class DeleteGenreCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly AppDbContext _context;

    public DeleteGenreCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
    }

     [Fact]
    public void WhenGenreIsNotFound_InvalidOperationException_ShouldBeThrown()
    {
        // Arrange
        DeleteGenreCommand command = new DeleteGenreCommand(_context);
        command.GenreId = 999; // Non-existent Genre ID
        
        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And.Message.Should().Be("Genre not found.");
    }

    [Fact]
    public void WhenGenreIsFound_GenreShouldBeSoftDeletedSuccessfully()
    {
        // Arrange
        var Genre = new Genre()
        {
            Name = "Test Genre",
            Description = "Test Description",
            IsActive = true
        };
        _context.Genres.Add(Genre);
        _context.SaveChanges();

        DeleteGenreCommand command = new DeleteGenreCommand(_context);
        command.GenreId = Genre.Id;

        // Act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // Assert
        var softDeletedGenre = _context.Genres.SingleOrDefault(b => b.Id == Genre.Id);
        softDeletedGenre.Should().NotBeNull();
        softDeletedGenre!.IsActive.Should().BeFalse();
    }
}
