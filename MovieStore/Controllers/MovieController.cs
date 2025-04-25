using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Schema;
using MovieStore.Implementation.Cqrs;

namespace MovieStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMediator mediator;
    public MoviesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("GetAll")]
    public async Task<ApiResponse<List<MovieResponse>>> GetAll()
    {
        var operation = new GetAllMoviesQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("GetById/{id}")]
    public async Task<ApiResponse<MovieResponse>> GetById([FromRoute] int id)
    {
        var operation = new GetMovieByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }


    [HttpPost]
    public async Task<ApiResponse<MovieResponse>> Post([FromBody] MovieRequest Movie)
    {
        var operation = new CreateMovieCommand(Movie);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut]
    public async Task<ApiResponse> Put(int Id,[FromBody] MovieRequest Movie)
    {
        var operation = new UpdateMovieCommand(Id,Movie);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete([FromRoute] int id)
    {
        var operation = new DeleteMovieCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }

}