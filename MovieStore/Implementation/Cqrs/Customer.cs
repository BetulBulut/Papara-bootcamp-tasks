using MediatR;
using MovieStore.Schema;


namespace MovieStore.Implementation.Cqrs;

public record GetAllCustomersQuery :IRequest<ApiResponse<List<CustomerResponse>>>;
public record GetCustomerByIdQuery(int Id) : IRequest<ApiResponse<CustomerResponse>>;
public record GetOrdersByCustomerIdQuery(int Id) : IRequest<ApiResponse<List<OrderResponse>>>;
public record CreateCustomerCommand(CustomerRequest customer) : IRequest<ApiResponse<CustomerResponse>>;
public record DeleteCustomerCommand(int Id) : IRequest<ApiResponse>;
public record CreateFavoriteGenreCommand(int Id,int GenreId) : IRequest<ApiResponse>;
public record AddOrderCommand(int CustomerId, int MovieId) : IRequest<ApiResponse<OrderResponse>>;