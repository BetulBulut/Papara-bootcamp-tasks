using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using MovieStore.Data;
using MovieStore.Implementation.Command;
using MovieStore.Implementation.Cqrs;
using MovieStore.Models;
using MovieStore.Schema;


namespace MovieStore.Tests.CommandTests;
public class DirectorCommandHandlerTests
{
    private readonly AppDbContext _context;
    private readonly Mock<IMapper> _mockMapper;
    private readonly DirectorCommandHandler _handler;

    public DirectorCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);
        _mockMapper = new Mock<IMapper>();
        _handler = new DirectorCommandHandler(_context, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_DeleteDirectorCommand_DirectorNotFound_ReturnsErrorResponse()
    {
        // Arrange
        var command = new DeleteDirectorCommand(1);
        _context.Directors.Add(new Director { Id = 2,FirstName = "John", LastName = "Doe", IsActive = true });
        await _context.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Director not found", result.Message);
    }

    [Fact]
    public async Task Handle_DeleteDirectorCommand_DirectorReferencedByMovies_ThrowsException()
    {
        // Arrange
        var Director = new Director { Id = 1, IsActive = true , FirstName = "John", LastName = "Doe" };
        var movie = new Movie {  Title = "Test Movie", IsActive = true , Id = 1 ,DirectorId = 1, Actors = new List<Actor>() };
        _context.Directors.Add(Director);
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(new DeleteDirectorCommand(1), default));
    }

    [Fact]
    public async Task Handle_DeleteDirectorCommand_SuccessfullySoftDeletesDirector()
    {
        // Arrange
        var Director = new Director { Id = 1, IsActive = true, FirstName = "John", LastName = "Doe" };
        _context.Directors.Add(Director);
        await _context.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(new DeleteDirectorCommand(1), default);

        // Assert
        Assert.True(result.Success);
        var deletedDirector = await _context.Directors.FindAsync(1);
        Assert.NotNull(deletedDirector);
        Assert.False(deletedDirector.IsActive);
    }


    [Fact]
    public async Task Handle_CreateDirectorCommand_SuccessfullyCreatesDirector()
    {
        // Arrange
        var Director = new Director { FirstName = "John", LastName = "Doe" };
        _mockMapper.Setup(m => m.Map<Director>(It.IsAny<DirectorRequest>())).Returns(Director);

        var command = new CreateDirectorCommand(new DirectorRequest { FirstName = "John", LastName = "Doe" });

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(await _context.Directors.FirstOrDefaultAsync(a => a.FirstName == "John" && a.LastName == "Doe"));
    }
}