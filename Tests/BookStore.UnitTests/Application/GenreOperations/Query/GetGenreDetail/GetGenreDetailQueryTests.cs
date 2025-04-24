using TestSetup;
using BookStore.Data;
using FluentAssertions;
using AutoMapper;
using BookStore.Application.GenreOperations.Query;

namespace Tests.BookStore.UnitTests.Application.GenreOperations.Query.GetGenreDetail;

public class GetGenreDetailQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetGenreDetailQueryTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

   [Fact]
    public void WhenGenreIsNotFound_InvalidOperationException_ShouldBeThrown()
    {
        // Arrange
        GetGenreDetailQuery command = new GetGenreDetailQuery(_context, _mapper);
        command.GenreId = 999; // Non-existent Genre ID

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And.Message.Should().Be("Genre not found.");
    }
}
