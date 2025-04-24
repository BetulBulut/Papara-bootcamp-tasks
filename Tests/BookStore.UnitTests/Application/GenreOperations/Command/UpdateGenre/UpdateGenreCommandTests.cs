using TestSetup;
using BookStore.Data;
using FluentAssertions;
using AutoMapper;
using BookStore.Application.GenreOperations.Command;
using BookStore.Models;

namespace Tests.BookStore.UnitTests.Application.GenreOperations.Command.UpdateGenre;

public class UpdateGenreCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UpdateGenreCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void WhenGenreIsNotFound_InvalidOperationException_ShouldBeThrown()
    {
        // Arrange
        UpdateGenreCommand command = new UpdateGenreCommand(_context, _mapper);
        command.GenreId = 999; // Non-existent Genre ID
        command.UpdatedGenre = new UpdateGenreModel() { Name = "NonExistentGenre", Description = "Description" };

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And.Message.Should().Be("Genre not found.");
    }

    [Fact]
    public void WhenGenreIsFound_GenreShouldBeUpdatedSuccessfully()
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

        UpdateGenreCommand command = new UpdateGenreCommand(_context, _mapper);
        command.GenreId = Genre.Id;
        command.UpdatedGenre = new UpdateGenreModel()
        {
            Name = "Updated Title",
            Description = "Updated Description"
        };

        // Act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // Assert
        var updatedGenre = _context.Genres.SingleOrDefault(b => b.Id == Genre.Id);
        updatedGenre.Should().NotBeNull();
        updatedGenre.Name.Should().Be("Updated Title");
        updatedGenre.Description.Should().Be("Updated Description");
        updatedGenre.IsActive.Should().Be(true);
    }
}
