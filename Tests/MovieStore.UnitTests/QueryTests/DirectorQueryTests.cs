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
public class DirectorQueryHandlerTests
{
    private readonly AppDbContext _context;
    private readonly Mock<IMapper> _mockMapper;
    private readonly DirectorQueryHandler _handler;

    public DirectorQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options); // InMemoryDatabase kullanımı
        _mockMapper = new Mock<IMapper>();
        _handler = new DirectorQueryHandler(_context, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_GetAllDirectorsQuery_ReturnsDirectors()
    {
        // Arrange
        var Directors = new List<Director>
        {
            new Director { Id = 1, FirstName = "John", LastName = "Doe", IsActive = true },
            new Director { Id = 2, FirstName = "Jane", LastName = "Smith", IsActive = true }
        };

        await _context.Directors.AddRangeAsync(Directors);
        await _context.SaveChangesAsync();

        _mockMapper.Setup(m => m.Map<List<DirectorResponse>>(It.IsAny<List<Director>>()))
            .Returns(new List<DirectorResponse>
            {
                new DirectorResponse { Id = 1, FirstName = "John", LastName = "Doe" },
                new DirectorResponse { Id = 2, FirstName = "Jane", LastName = "Smith" }
            });

        // Act
        var result = await _handler.Handle(new GetAllDirectorsQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Response);
        Assert.Equal(2, result.Response.Count);
    }


  
    [Fact]
    public async Task Handle_GetAllDirectorsQuery_NoDirectorsFound_ReturnsError()
    {
        // Arrange
        // Veritabanında hiç aktör yok, bu yüzden ekleme yapılmıyor.

        // Act
        var result = await _handler.Handle(new GetAllDirectorsQuery(), CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("No Directors found", result.Message);
    }

    [Fact]
    public async Task Handle_GetDirectorByIdQuery_ReturnsDirector()
    {
        // Arrange
        var Director = new Director { Id = 1, FirstName = "John", LastName = "Doe", IsActive = true };
        await _context.Directors.AddAsync(Director);
        await _context.SaveChangesAsync();

        _mockMapper.Setup(m => m.Map<DirectorResponse>(It.IsAny<Director>()))
            .Returns(new DirectorResponse { Id = 1, FirstName = "John", LastName = "Doe" });

        // Act
        var result = await _handler.Handle(new GetDirectorByIdQuery(1), CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Response);
        Assert.Equal("John", result.Response.FirstName);
    }

}