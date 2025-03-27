using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ModelUse.Models;
using ModelUse.Services;


namespace ModelUse.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{

    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>>  GetBooks()
    {   
        var books = await _bookService.GetAll();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBookById(int id)
    {
        var book = await _bookService.GetById(id);
        if (book == null)
            return NotFound(new { message = "Book not found" });

        return Ok(book);
        
    }

    [HttpPost]
    public async Task<ActionResult<Book>>  AddBook([FromBody] Book newBook)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        await _bookService.Add(newBook);
        return Ok(newBook);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] Book updatedBook)
    {
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        var existingBook = await _bookService.GetById(id); 
        if (existingBook == null)
            return NotFound(new { message = "Book not found" });

        
        existingBook.Title = updatedBook.Title;
        existingBook.Author = updatedBook.Author;
        existingBook.Price = updatedBook.Price;
        
        await _bookService.Update(existingBook); 

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book =await _bookService.GetById(id);
        if (book == null)
            return NotFound(new { message = "Book not found" });

        await _bookService.Delete(id);
        return NoContent();
    }
}

