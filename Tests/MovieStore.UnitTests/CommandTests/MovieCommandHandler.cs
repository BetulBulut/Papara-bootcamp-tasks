using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using MovieStore.Data;
using MovieStore.Implementation.Command;
using MovieStore.Implementation.Cqrs;
using MovieStore.Models;
using MovieStore.Schema;


namespace MovieStore.Tests.CommandTests;
public class MovieCommandHandlerTests
{
    private readonly AppDbContext _context;
    private readonly Mock<IMapper> _mockMapper;
    private readonly MovieCommandHandler _handler;

    public MovieCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);
        _mockMapper = new Mock<IMapper>();
        _handler = new MovieCommandHandler(_context, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_DeleteMovieCommand_MovieNotFound_ReturnsErrorResponse()
    {
        // Arrange
        var command = new DeleteMovieCommand(1);
        _context.Movies.Add(new Movie { Id = 2, Title = "Test Movie", IsActive = true , DirectorId = 1, Actors = new List<Actor>() });
        await _context.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Movie not found", result.Message);
    }

    [Fact]
    public async Task Handle_DeleteMovieCommand_MovieReferencedByMovies_ThrowsException()
    {
        // Arrange
        var Movie = new Movie { Id = 1, Title = "Test Movie1", IsActive = true , DirectorId = 1, Actors = new List<Actor>() };
        var order = new Order { MovieId = Movie.Id, IsActive = true,PurchaseDate = DateTime.UtcNow ,PriceAtPurchase = 10.0m, CustomerId = 1};
        _context.Movies.Add(Movie);
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(new DeleteMovieCommand(1), default));
    }

  
}