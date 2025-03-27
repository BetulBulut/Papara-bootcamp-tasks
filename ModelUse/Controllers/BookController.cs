using Microsoft.AspNetCore.Mvc;
using ModelUse.Models;
using MediatR;
using ModelUse.Schema;
using ModelUse.Implementation.Cqrs;


namespace ModelUse.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{

    private readonly IMediator mediator;
    public BookController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet("GetAll")]
    public async Task<ApiResponse<List<BookResponse>>> GetAllBooks()
    {
        var operation = new GetAllBooksQuery();
        var result = await mediator.Send(operation);
        return result;
    }
    
    [HttpGet("GetById/{id}")]
    public async Task<ApiResponse<BookResponse>> GetBookById([FromRoute] int id)
    {
        var operation = new GetBookByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }

     [HttpPost]
    public async Task<ApiResponse<BookResponse>> AddBook([FromBody] BookRequest book)
    {
        var operation = new CreateBookCommand(book);
        var result = await mediator.Send(operation);
        return result;
    }


     [HttpPut("{id}")]
    public async Task<ApiResponse> UpdateBook([FromRoute] int id, [FromBody] BookRequest book)
    {
        var operation = new UpdateBookCommand(id,book);
        var result = await mediator.Send(operation);
        return result;
    }[HttpDelete("{id}")]
    public async Task<ApiResponse> DeleteBook([FromRoute] int id)
    {
        var operation = new DeleteBookCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }
}

