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
public class MovieQueryHandlerTests
{
    private readonly AppDbContext _context;
    private readonly Mock<IMapper> _mockMapper;
    private readonly MovieQueryHandler _handler;

    public MovieQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options); // InMemoryDatabase kullanımı
        _mockMapper = new Mock<IMapper>();
        _handler = new MovieQueryHandler(_context, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_GetAllMoviesQuery_ReturnsMovies()
    {
        // Arrange
        var Movies = new List<Movie>
        {
            new Movie { Id = 1, Title = "Test Movie", IsActive = true, DirectorId = 1, Actors = new List<Actor>() },
            new Movie { Id = 2, Title = "Another Movie", IsActive = true, DirectorId = 2, Actors = new List<Actor>() }
           
        };

        await _context.Movies.AddRangeAsync(Movies);
        await _context.SaveChangesAsync();

        _mockMapper.Setup(m => m.Map<List<MovieResponse>>(It.IsAny<List<Movie>>()))
            .Returns(new List<MovieResponse>
            {
                new MovieResponse { Id = 1, Title = "Test Movie", IsActive = true},
                new MovieResponse { Id = 2, Title = "Another Movie", IsActive = true }
            });

        // Act
        var result = await _handler.Handle(new GetAllMoviesQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Response);
        Assert.Equal(2, result.Response.Count);
    }


  
    [Fact]
    public async Task Handle_GetAllMoviesQuery_NoMoviesFound_ReturnsError()
    {
        // Arrange
        // Veritabanında hiç aktör yok, bu yüzden ekleme yapılmıyor.

        // Act
        var result = await _handler.Handle(new GetAllMoviesQuery(), CancellationToken.None);

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public async Task Handle_GetMovieByIdQuery_ReturnsMovie()
    {
        // Arrange
        var Movie = new Movie { Id = 1, Title = "Test Movie", IsActive = true, DirectorId = 1, Actors = new List<Actor>() };
        await _context.Movies.AddAsync(Movie);
        await _context.SaveChangesAsync();

        _mockMapper.Setup(m => m.Map<MovieResponse>(It.IsAny<Movie>()))
            .Returns(new MovieResponse { Id = 1, Title = "Test Movie", IsActive = true});

        // Act
        var result = await _handler.Handle(new GetMovieByIdQuery(1), CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Response);
        Assert.Equal("Test Movie", result.Response.Title);
    }

}