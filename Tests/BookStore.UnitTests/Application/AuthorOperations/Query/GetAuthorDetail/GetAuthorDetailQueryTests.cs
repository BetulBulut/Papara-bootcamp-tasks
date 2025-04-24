using TestSetup;
using BookStore.Data;
using FluentAssertions;
using AutoMapper;
using BookStore.Application.AuthorOperations.Query;

namespace Tests.BookStore.UnitTests.Application.AuthorOperations.Query.GetAuthorDetail;

public class GetAuthorDetailQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetAuthorDetailQueryTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

   [Fact]
    public void WhenAuthorIsNotFound_InvalidOperationException_ShouldBeThrown()
    {
        // Arrange
        GetAuthorDetailQuery command = new GetAuthorDetailQuery(_context, _mapper);
        command.AuthorId = 999; // Non-existent Author ID

        // Act & Assert
        FluentActions
            .Invoking(() => command.Handle())
            .Should().Throw<InvalidOperationException>()
            .And.Message.Should().Be("Author not found.");
    }
}
