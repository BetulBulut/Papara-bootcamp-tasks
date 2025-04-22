using Microsoft.AspNetCore.Mvc;
using BookStore.Data;
using BookStore.BookOperations;

namespace BookStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly AppDbContext _context;

    public BookController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetBooks()
    {
        GetBooksQuery query = new GetBooksQuery(_context);
        var result = query.Handle();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetBookDetail(int id)
    {
        GetBookDetailQuery query = new GetBookDetailQuery(_context);
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
        CreateBookCommand command = new CreateBookCommand(_context);
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
        UpdateBookCommand command = new UpdateBookCommand(_context);
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
