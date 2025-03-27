using MediatR;
using ModelUse.Schema;

namespace ModelUse.Implementation.Cqrs;

public record GetAllBooksQuery : IRequest<ApiResponse<List<BookResponse>>>;
public record GetBookByIdQuery(int Id) :  IRequest<ApiResponse<BookResponse>>;
public record CreateBookCommand(BookRequest book) : IRequest<ApiResponse<BookResponse>>;
public record UpdateBookCommand(int Id, BookRequest book) : IRequest<ApiResponse>;
public record DeleteBookCommand(int Id) : IRequest<ApiResponse>;