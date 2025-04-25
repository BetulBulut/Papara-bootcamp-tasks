using MediatR;
using Microsoft.AspNetCore.Mvc;

using MovieStore.Schema;
using MovieStore.Implementation.Cqrs;

namespace MovieStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DirectorsController : ControllerBase
{
    private readonly IMediator mediator;
    public DirectorsController(IMediator mediator)
    {
        this.mediator = mediator;
    }


    [HttpGet("GetAll")]
    public async Task<ApiResponse<List<DirectorResponse>>> GetAll()
    {
        var operation = new GetAllDirectorsQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("GetById/{id}")]
    public async Task<ApiResponse<DirectorResponse>> GetById([FromRoute] int id)
    {
        var operation = new GetDirectorByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }
   
    [HttpPost]
    public async Task<ApiResponse<DirectorResponse>> Post([FromBody] DirectorRequest Director)
    {
        var operation = new CreateDirectorCommand(Director);
        var result = await mediator.Send(operation);
        return result;
    }
    [HttpPut]
    public async Task<ApiResponse> Put(int Id,[FromBody] DirectorRequest Director)
    {
        var operation = new UpdateDirectorCommand(Id,Director);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete([FromRoute] int id)
    {
        var operation = new DeleteDirectorCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }

}