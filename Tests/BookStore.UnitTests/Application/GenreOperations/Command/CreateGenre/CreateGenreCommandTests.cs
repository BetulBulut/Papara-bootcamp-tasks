using TestSetup;
using BookStore.Data;
using FluentAssertions;
using AutoMapper;
using BookStore.Application.GenreOperations.Command;
using BookStore.Models;

namespace Tests.BookStore.UnitTests.Application.GenreOperations.Command.CreateGenre;

public class CreateGenreCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CreateGenreCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void WhenAlreadyExistGenreTitleIsGiven_InvalidOperationException_ShouldBeReturn()
    {
        // Arrange (Hazırlık)
        var Genre = new Genre()
        {
            Description = "Test_WhenAlreadyExistGenreTitleIsGiven_InvalidOperationException_ShouldBeReturn",
            Name = "Test Genre",
        };
        _context.Genres.Add(Genre);
        _context.SaveChanges();

        CreateGenreCommand command = new CreateGenreCommand(_context, _mapper);
        command.Model = new CreateGenreModel() { Name = Genre.Name };

        // Act & Assert (Çalıştırma - Doğrulama)
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And.Message.Should().Be("Genre already exists.");
    }

    [Fact]
    public void WhenValidInputsAreGiven_Genre_ShouldBeCreated()
    {
        //arrange
        CreateGenreCommand command = new CreateGenreCommand(_context, _mapper);
        CreateGenreModel model = new CreateGenreModel() { Name = "Genre111", Description = "Description111" };
        command.Model = model;

        //act
        FluentActions.Invoking(() => command.Handle()).Invoke();

        //assert
        var Genre = _context.Genres.SingleOrDefault(Genre => Genre.Name == model.Name);
        Genre.Should().NotBeNull();
        Genre.Description.Should().Be(model.Description);
        Genre.IsActive.Should().Be(true);
    }
}
