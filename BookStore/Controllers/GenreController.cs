using Microsoft.AspNetCore.Mvc;
using BookStore.Data;
using BookStore.Application.GenreOperations.Query;
using BookStore.Application.GenreOperations.Command;
using AutoMapper;

namespace GenreStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenreController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GenreController(AppDbContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }
   
    [HttpGet]
    public IActionResult GetGenres()
    {
        GetGenresQuery query = new GetGenresQuery(_context, _mapper);
        var result = query.Handle();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetGenreDetail(int id)
    {
        GetGenreDetailQuery query = new GetGenreDetailQuery(_context, _mapper);
        query.GenreId = id;

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
    public IActionResult AddGenre([FromBody] CreateGenreModel newGenre)
    {
        CreateGenreCommand command = new CreateGenreCommand(_context, _mapper);
        command.Model = newGenre;

        try
        {
            command.Handle();
            return Ok("Genre successfully added.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateGenre(int id, [FromBody] UpdateGenreModel updatedGenre)
    {
        UpdateGenreCommand command = new UpdateGenreCommand(_context, _mapper);
        command.GenreId = id;
        command.UpdatedGenre = updatedGenre;

        try
        {
            command.Handle();
            return Ok("Genre successfully updated.");
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteGenre(int id)
    {
        DeleteGenreCommand command = new DeleteGenreCommand(_context);
        command.GenreId = id;

        try
        {
            command.Handle();
            return Ok("Genre successfully deleted.");
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
