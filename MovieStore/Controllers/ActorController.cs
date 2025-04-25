using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Schema;
using MovieStore.Implementation.Cqrs;

namespace MovieStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActorsController : ControllerBase
{
    private readonly IMediator mediator;
    public ActorsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("GetAll")]
    public async Task<ApiResponse<List<ActorResponse>>> GetAll()
    {
        var operation = new GetAllActorsQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("GetById/{id}")]
    public async Task<ApiResponse<ActorResponse>> GetById([FromRoute] int id)
    {
        var operation = new GetActorByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }
   
    [HttpPost]
    public async Task<ApiResponse<ActorResponse>> Post([FromBody] ActorRequest Actor)
    {
        var operation = new CreateActorCommand(Actor);
        var result = await mediator.Send(operation);
        return result;
    }
    [HttpPut]
    public async Task<ApiResponse> Put(int Id,[FromBody] ActorRequest Actor)
    {
        var operation = new UpdateActorCommand(Id,Actor);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete([FromRoute] int id)
    {
        var operation = new DeleteActorCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }

}