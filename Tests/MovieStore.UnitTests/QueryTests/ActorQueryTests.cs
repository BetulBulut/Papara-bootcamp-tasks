using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using MovieStore.Data;
using MovieStore.Implementation.Cqrs;
using MovieStore.Implementation.Query;
using MovieStore.Models;
using MovieStore.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MovieStore.Tests.QueryTests;
public class ActorQueryHandlerTests
{
    private readonly AppDbContext _context;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ActorQueryHandler _handler;

    public ActorQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options); // InMemoryDatabase kullanımı
        _mockMapper = new Mock<IMapper>();
        _handler = new ActorQueryHandler(_context, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_GetAllActorsQuery_ReturnsActors()
    {
        // Arrange
        var actors = new List<Actor>
        {
            new Actor { Id = 1, FirstName = "John", LastName = "Doe", IsActive = true },
            new Actor { Id = 2, FirstName = "Jane", LastName = "Smith", IsActive = true }
        };

        await _context.Actors.AddRangeAsync(actors);
        await _context.SaveChangesAsync();

        _mockMapper.Setup(m => m.Map<List<ActorResponse>>(It.IsAny<List<Actor>>()))
            .Returns(new List<ActorResponse>
            {
                new ActorResponse { Id = 1, FirstName = "John", LastName = "Doe" },
                new ActorResponse { Id = 2, FirstName = "Jane", LastName = "Smith" }
            });

        // Act
        var result = await _handler.Handle(new GetAllActorsQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Response);
        Assert.Equal(2, result.Response.Count);
    }


  
    [Fact]
    public async Task Handle_GetAllActorsQuery_NoActorsFound_ReturnsError()
    {
        // Arrange
        // Veritabanında hiç aktör yok, bu yüzden ekleme yapılmıyor.

        // Act
        var result = await _handler.Handle(new GetAllActorsQuery(), CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("No Actors found", result.Message);
    }

    [Fact]
    public async Task Handle_GetActorByIdQuery_ReturnsActor()
    {
        // Arrange
        var actor = new Actor { Id = 1, FirstName = "John", LastName = "Doe", IsActive = true };
        await _context.Actors.AddAsync(actor);
        await _context.SaveChangesAsync();

        _mockMapper.Setup(m => m.Map<ActorResponse>(It.IsAny<Actor>()))
            .Returns(new ActorResponse { Id = 1, FirstName = "John", LastName = "Doe" });

        // Act
        var result = await _handler.Handle(new GetActorByIdQuery(1), CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Response);
        Assert.Equal("John", result.Response.FirstName);
    }

}