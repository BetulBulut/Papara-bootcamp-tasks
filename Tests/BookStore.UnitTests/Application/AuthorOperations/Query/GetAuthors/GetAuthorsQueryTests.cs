using TestSetup;
using BookStore.Data;
using FluentAssertions;
using AutoMapper;
using BookStore.Application.AuthorOperations.Query;
using BookStore.Models;

namespace Tests.BookStore.UnitTests.Application.AuthorOperations.Query.GetAuthors;

public class GetAuthorsQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetAuthorsQueryTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void WhenAuthorsExist_AuthorsShouldBeRetrievedSuccessfully()
    {
        // Arrange
        var Authors = new List<Author>
        {
            new Author { FirstName = "Author 1",LastName = "Lastname 1", BirthDate = new DateTime(1990, 1, 1), IsActive=true},
            new Author { FirstName = "Author 2",LastName = "Lastname 2", BirthDate = new DateTime(1990, 1, 1), IsActive=true},
        };
        _context.Authors.AddRange(Authors);
        _context.SaveChanges();

        GetAuthorsQuery query = new GetAuthorsQuery(_context, _mapper);

        // Act
        var result = query.Handle();

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().BeGreaterThanOrEqualTo(2);
        result.Should().Contain(b => b.FirstName == "Author 1"&& b.LastName == "Lastname 1" && b.BirthDate == new DateTime(1990, 1, 1));
        result.Should().Contain(b => b.FirstName == "Author 2"&& b.LastName == "Lastname 2" && b.BirthDate == new DateTime(1990, 1, 1));
    }
 
}
