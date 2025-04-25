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
public class CustomerQueryHandlerTests
{
    private readonly AppDbContext _context;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CustomerQueryHandler _handler;

    public CustomerQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options); // InMemoryDatabase kullanımı
        _mockMapper = new Mock<IMapper>();
        _handler = new CustomerQueryHandler(_context, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_GetAllCustomersQuery_ReturnsCustomers()
    {
        // Arrange
        var Customers = new List<Customer>
        {
            new Customer { Id = 1, FirstName = "John", LastName = "Doe", IsActive = true , Username = "johndoe", PasswordHash = "password",Secret="secret", FavoriteGenres = new List<GenreEnum>() },
            new Customer { Id = 2, FirstName = "Jane", LastName = "Smith", IsActive = true , Username = "janesmith", PasswordHash = "password",Secret="secret", FavoriteGenres = new List<GenreEnum>() }
        };

        await _context.Customers.AddRangeAsync(Customers);
        await _context.SaveChangesAsync();

        _mockMapper.Setup(m => m.Map<List<CustomerResponse>>(It.IsAny<List<Customer>>()))
            .Returns(new List<CustomerResponse>
            {
                new CustomerResponse { Id = 1, FirstName = "John", LastName = "Doe", Username = "johndoe",FavoriteGenres = new List<GenreEnum>() },
                new CustomerResponse { Id = 2, FirstName = "Jane", LastName = "Smith", Username = "janesmith", FavoriteGenres = new List<GenreEnum>() }
            });

        // Act
        var result = await _handler.Handle(new GetAllCustomersQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Response);
        Assert.Equal(2, result.Response.Count);
    }


  
    [Fact]
    public async Task Handle_GetAllCustomersQuery_NoCustomersFound_ReturnsError()
    {
        // Arrange
        // Veritabanında hiç aktör yok, bu yüzden ekleme yapılmıyor.

        // Act
        var result = await _handler.Handle(new GetAllCustomersQuery(), CancellationToken.None);

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public async Task Handle_GetCustomerByIdQuery_ReturnsCustomer()
    {
        // Arrange
        var Customer = new Customer { Id = 1, FirstName = "John", LastName = "Doe", IsActive = true, Username = "johndoe", PasswordHash = "password",Secret="secret", FavoriteGenres = new List<GenreEnum>() };
        await _context.Customers.AddAsync(Customer);
        await _context.SaveChangesAsync();

        _mockMapper.Setup(m => m.Map<CustomerResponse>(It.IsAny<Customer>()))
            .Returns(new CustomerResponse { Id = 1, FirstName = "John", LastName = "Doe", Username = "johndoe",FavoriteGenres = new List<GenreEnum>() });

        // Act
        var result = await _handler.Handle(new GetCustomerByIdQuery(1), CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Response);
        Assert.Equal("John", result.Response.FirstName);
    }

}