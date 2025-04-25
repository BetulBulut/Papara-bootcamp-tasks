using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MovieStore.Schema;
using MovieStore.Implementation.Cqrs;
using MovieStore.Models;
using System.Security.Claims;

namespace MovieStore.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator mediator;
    public CustomersController(IMediator mediator)
    {
        this.mediator = mediator;
    }


    [HttpGet("GetAll")]
    public async Task<ApiResponse<List<CustomerResponse>>> GetAll()
    {
        var operation = new GetAllCustomersQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("GetById/{id}")]
    public async Task<ApiResponse<CustomerResponse>> GetById([FromRoute] int id)
    {
        var operation = new GetCustomerByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }
    [HttpGet("GetOrdersByCustomerId/{id}")]
    public async Task<ApiResponse<List<OrderResponse>>> GetOrdersByCustomerId([FromRoute] int id)
    {
        var operation = new GetOrdersByCustomerIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost]
    public async Task<ApiResponse<CustomerResponse>> Post([FromBody] CustomerRequest customer)
    {
        var operation = new CreateCustomerCommand(customer);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete([FromRoute] int id)
    {
        var operation = new DeleteCustomerCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }

     [HttpGet("GetGenres")]
    public IActionResult GetGenres()
    {
        var genres = Enum.GetValues(typeof(GenreEnum))
                         .Cast<GenreEnum>()
                         .Select(g => new { Id = (int)g, Name = g.ToString() })
                         .ToList();

        return Ok(genres);
    }

    [HttpPost("AddFavoriteGenre")]
    public async Task<ApiResponse> AddFavoriteGenre([FromBody] CreateFavoriteGenreCommand command)
    {
        var operation = new CreateFavoriteGenreCommand(command.Id, command.GenreId);
        var result = await mediator.Send(operation);
        return result;
    }

    [Authorize(Roles = "Customer")]
    [HttpPost("BuyMovie")]
    public async Task<ApiResponse<OrderResponse>> BuyMovie(int movieId)
    {   
        var customerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var operation = new AddOrderCommand(customerId,movieId);
        var result = await mediator.Send(operation);
        return result;
    }
    

}