using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using MovieStore.Data;
using MovieStore.Implementation.Command;
using MovieStore.Implementation.Cqrs;
using MovieStore.Models;
using MovieStore.Schema;
using Xunit;

namespace MovieStore.Tests.CommandTests;
public class CustomerCommandHandlerTests
{
    private readonly AppDbContext _context;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CustomerCommandHandler _handler;

    public CustomerCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);
        _mockMapper = new Mock<IMapper>();
        _handler = new CustomerCommandHandler(_context, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_DeleteCustomerCommand_CustomerNotFound_ReturnsErrorResponse()
    {
        // Arrange
        var command = new DeleteCustomerCommand(1);
        _context.Customers.Add(new Customer { Id = 2,FirstName = "John1", LastName = "Doe1", IsActive = true , Username = "johndoe1", PasswordHash = "password1",Secret="secret1",FavoriteGenres = new List<GenreEnum>() }); ;
        await _context.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Customer not found", result.Message);
    }


    [Fact]
    public async Task Handle_DeleteCustomerCommand_SuccessfullySoftDeletesCustomer()
    {
        // Arrange
        var Customer = new Customer { Id = 1, IsActive = true, FirstName = "John", LastName = "Doe" , Username = "johndoe", PasswordHash = "password",Secret="secret", FavoriteGenres = new List<GenreEnum>() };
        _context.Customers.Add(Customer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _handler.Handle(new DeleteCustomerCommand(1), default);

        // Assert
        Assert.True(result.Success);
        var deletedCustomer = await _context.Customers.FindAsync(1);
        Assert.NotNull(deletedCustomer);
        Assert.False(deletedCustomer.IsActive);
    }


    [Fact]
    public async Task Handle_CreateCustomerCommand_SuccessfullyCreatesCustomer()
    {
        // Arrange
        var Customer = new Customer { FirstName = "John2", LastName = "Doe2", Username = "johndoe2", PasswordHash = "password2",Secret="secret2", FavoriteGenres = new List<GenreEnum>() };
        _mockMapper.Setup(m => m.Map<Customer>(It.IsAny<CustomerRequest>())).Returns(Customer);

        var command = new CreateCustomerCommand(new CustomerRequest { FirstName = "John2", LastName = "Doe2", Username = "johndoe2"});

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(await _context.Customers.FirstOrDefaultAsync(a => a.FirstName == "John2" && a.LastName == "Doe2"));
    }
}