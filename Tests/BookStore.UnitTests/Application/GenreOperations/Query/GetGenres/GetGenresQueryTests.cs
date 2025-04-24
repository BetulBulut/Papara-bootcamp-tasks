using TestSetup;
using BookStore.Data;
using FluentAssertions;
using AutoMapper;
using BookStore.Application.GenreOperations.Query;
using BookStore.Models;

namespace Tests.BookStore.UnitTests.Application.GenreOperations.Query.GetGenres;

public class GetGenresQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetGenresQueryTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void WhenGenresExist_GenresShouldBeRetrievedSuccessfully()
    {
        // Arrange
        var Genres = new List<Genre>
        {
            new Genre { Name = "Genre 1",Description = "Description 1" , IsActive=true},
            new Genre { Name = "Genre 2",Description = "Description 2" , IsActive=true}
            
        };
        _context.Genres.AddRange(Genres);
        _context.SaveChanges();

        GetGenresQuery query = new GetGenresQuery(_context, _mapper);

        // Act
        var result = query.Handle();

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().BeGreaterThanOrEqualTo(2);
        result.Should().Contain(b => b.Name == "Genre 1");
        result.Should().Contain(b => b.Name == "Genre 2");
    }
 
}
