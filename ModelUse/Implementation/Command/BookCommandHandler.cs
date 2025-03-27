using MediatR;
using Microsoft.EntityFrameworkCore;
using ModelUse.Models;
using ModelUse.Data;
using ModelUse.Implementation.Cqrs;
using ModelUse.Schema;

namespace WebApi.Implementation.Command;

public class BookCommandHandler : 
IRequestHandler<CreateBookCommand, ApiResponse<BookResponse>>, 
IRequestHandler<UpdateBookCommand, ApiResponse>, 
IRequestHandler<DeleteBookCommand, ApiResponse>
{
    private readonly AppDbContext _context;
    public BookCommandHandler(AppDbContext context)
    {
        this._context = context;
    }

    public async Task<ApiResponse<BookResponse>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        
        var book = new Book
        {
            Title = request.book.Title,
            Author = request.book.Author,
            Price = request.book.Price,
            PublishedDate=DateTime.Now,
            ISBN=new Random().Next(1000000, 999999999).ToString()
        };
        var entity = await _context.Books.AddAsync(book, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        var response = new BookResponse
        {
            Id = entity.Entity.Id,
            Title = entity.Entity.Title,
            Author = entity.Entity.Author,
            Price = entity.Entity.Price,
            PublishedDate = entity.Entity.PublishedDate,
            ISBN = entity.Entity.ISBN
        };
        return new ApiResponse<BookResponse>(response);
    }

    public async Task<ApiResponse> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Set<Book>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            return new ApiResponse("Book not found");

        entity.Title = request.book.Title;
        entity.Author = request.book.Author;
        entity.Price = request.book.Price;

        await _context.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
       var entity = await _context.Set<Book>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            return new ApiResponse("Book not found");

        _context.Books.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}