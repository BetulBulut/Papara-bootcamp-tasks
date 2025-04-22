using Microsoft.AspNetCore.Mvc;
using BookStore.Data;
using BookStore.Application.BookOperations.Command;
using BookStore.Application.BookOperations.Query;
using BookStore.Application.BookOperations.Query.GetBooks;
using BookStore.Application.BookOperations.Query.GetBookDetail;
using BookStore.Application.BookOperations.Command.CreateBook;
using BookStore.Application.BookOperations.Command.UpdateBook;
using BookStore.Application.BookOperations.Command.DeleteBook;
using AutoMapper;

namespace BookStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public BookController(AppDbContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }
   
    [HttpGet]
    public IActionResult GetBooks()
    {
        GetBooksQuery query = new GetBooksQuery(_context, _mapper);
        var result = query.Handle();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetBookDetail(int id)
    {
        GetBookDetailQuery query = new GetBookDetailQuery(_context, _mapper);
        query.BookId = id;

        try
        {
            var result = query.Handle();
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public IActionResult AddBook([FromBody] CreateBookModel newBook)
    {
        CreateBookCommand command = new CreateBookCommand(_context, _mapper);
        command.Model = newBook;

        try
        {
            command.Handle();
            return Ok("Book successfully added.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateBook(int id, [FromBody] UpdateBookModel updatedBook)
    {
        UpdateBookCommand command = new UpdateBookCommand(_context, _mapper);
        command.BookId = id;
        command.UpdatedBook = updatedBook;

        try
        {
            command.Handle();
            return Ok("Book successfully updated.");
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(int id)
    {
        DeleteBookCommand command = new DeleteBookCommand(_context);
        command.BookId = id;

        try
        {
            command.Handle();
            return Ok("Book successfully deleted.");
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
