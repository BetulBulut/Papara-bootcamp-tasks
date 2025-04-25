using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using MovieStore.Data;
using MovieStore.Implementation.Command;
using MovieStore.Implementation.Cqrs;
using MovieStore.Models;
using MovieStore.Schema;


namespace MovieStore.Tests.CommandTests;
public class ActorCommandHandlerTests
{
    private readonly AppDbContext _context;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ActorCommandHandler _handler;

    public ActorCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);
        _mockMapper = new Mock<IMapper>();
        _handler = new ActorCommandHandler(_context, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_DeleteActorCommand_ActorNotFound_ReturnsErrorResponse()
    {
        // Arrange
        var command = new DeleteActorCommand(1);
        _context.Actors.Add(new Actor { Id = 2,FirstName = "John", LastName = "Doe", IsActive = true });
        await _context.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Actor not found", result.Message);
    }

    [Fact]
    public async Task Handle_DeleteActorCommand_ActorReferencedByMovies_ThrowsException()
    {
        // Arrange
        var actor = new Actor { Id = 1, IsActive = true , FirstName = "John", LastName = "Doe" };
        var movie = new Movie { Actors = new List<Actor> { actor }, Title = "Test Movie", IsActive = true , Id = 1 ,DirectorId = 1};
        _context.Actors.Add(actor);
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(new DeleteActorCommand(1), default));
    }

    [Fact]
    public async Task Handle_DeleteActorCommand_SuccessfullySoftDeletesActor()
    {
        // Arrange
        var actor = new Actor { Id = 1, IsActive = true, FirstName = "John", LastName = "Doe" };
        _context.Actors.Add(actor);
        await _context.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(new DeleteActorCommand(1), default);

        // Assert
        Assert.True(result.Success);
        var deletedActor = await _context.Actors.FindAsync(1);
        Assert.NotNull(deletedActor);
        Assert.False(deletedActor.IsActive);
    }


    [Fact]
    public async Task Handle_CreateActorCommand_SuccessfullyCreatesActor()
    {
        // Arrange
        var actor = new Actor { FirstName = "John", LastName = "Doe" };
        _mockMapper.Setup(m => m.Map<Actor>(It.IsAny<ActorRequest>())).Returns(actor);

        var command = new CreateActorCommand(new ActorRequest { FirstName = "John", LastName = "Doe" });

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(await _context.Actors.FirstOrDefaultAsync(a => a.FirstName == "John" && a.LastName == "Doe"));
    }
}