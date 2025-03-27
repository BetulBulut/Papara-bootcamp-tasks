using MediatR;
using Microsoft.EntityFrameworkCore;
using ModelUse.Data;
using ModelUse.Implementation.Cqrs;
using ModelUse.Models;
using ModelUse.Schema;
namespace WebApi.Implementation.Query;

public class CustomerQueryHandler :
IRequestHandler<GetAllBooksQuery, ApiResponse<List<BookResponse>>>,
IRequestHandler<GetBookByIdQuery, ApiResponse<BookResponse>>
{
    private readonly AppDbContext _context;

    public CustomerQueryHandler(AppDbContext context)
    {
        this._context = context;
    }

    public async Task<ApiResponse<List<BookResponse>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var customers = await _context.Set<Book>().ToListAsync(cancellationToken);
        var response = customers.Select(c => new BookResponse
        {
            Id = c.Id,
            Title = c.Title,
            Author = c.Author,
            ISBN = c.ISBN,
            PublishedDate = c.PublishedDate,
            Price = c.Price
        }).ToList();
        return new ApiResponse<List<BookResponse>>(response);
    }

    public async Task<ApiResponse<BookResponse>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _context.Set<Book>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (customer == null)
            return new ApiResponse<BookResponse>("Book not found");
        var response = new BookResponse
        {
            Id = customer.Id,
            Title = customer.Title,
            Author = customer.Author,
            ISBN = customer.ISBN,
            PublishedDate = customer.PublishedDate,
            Price = customer.Price
        };
        return new ApiResponse<BookResponse>(response);
    }
}