using TestSetup;
using BookStore.Data;
using FluentAssertions;
using AutoMapper;
using BookStore.Application.BookOperations.Query.GetBookDetail;

namespace Tests.BookStore.UnitTests.Application.BookOperations.Query.GetBookDetail;

public class GetBookDetailQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetBookDetailQueryTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

   [Fact]
    public void WhenBookIsNotFound_InvalidOperationException_ShouldBeThrown()
    {
        // Arrange
        GetBookDetailQuery command = new GetBookDetailQuery(_context, _mapper);
        command.BookId = 999; // Non-existent book ID

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And.Message.Should().Be("Book not found.");
    }
}
