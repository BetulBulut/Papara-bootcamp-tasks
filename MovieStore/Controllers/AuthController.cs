using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Schema;
using MovieStore.Implementation.Cqrs;

namespace MovieStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator mediator;
    public AuthController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("Login")]
    public async Task<ApiResponse<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
    {
        var operation = new LoginQuery(loginRequest);
        var result = await mediator.Send(operation);
        return result;
    }
}