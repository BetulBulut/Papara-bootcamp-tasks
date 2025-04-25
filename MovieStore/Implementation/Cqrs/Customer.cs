using MediatR;
using MovieStore.Schema;


namespace MovieStore.Implementation.Cqrs;

public record GetAllCustomersQuery :IRequest<ApiResponse<List<CustomerResponse>>>;
public record GetCustomerByIdQuery(int Id) : IRequest<ApiResponse<CustomerResponse>>;
public record CreateCustomerCommand(CustomerRequest customer) : IRequest<ApiResponse<CustomerResponse>>;
public record DeleteCustomerCommand(int Id) : IRequest<ApiResponse>;