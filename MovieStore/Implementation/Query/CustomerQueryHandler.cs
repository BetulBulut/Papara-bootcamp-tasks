using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStore.Data;
using MovieStore.Implementation.Cqrs;
using MovieStore.Schema;


namespace MovieStore.Implementation.Query;

public class CustomerQueryHandler :
IRequestHandler<GetAllCustomersQuery, ApiResponse<List<CustomerResponse>>>,
IRequestHandler<GetCustomerByIdQuery, ApiResponse<CustomerResponse>>,
IRequestHandler<GetOrdersByCustomerIdQuery, ApiResponse<List<OrderResponse>>>
{
    private readonly AppDbContext context;
    private readonly IMapper mapper;

    public CustomerQueryHandler( AppDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<CustomerResponse>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await context.Customers.Where(x => x.IsActive==true).ToListAsync(cancellationToken);
        if (customers == null || customers.Count == 0)
        {
            return new ApiResponse<List<CustomerResponse>>("No customers found");
        }
        var mapped = mapper.Map<List<CustomerResponse>>(customers);
        return new ApiResponse<List<CustomerResponse>>(mapped);
    }

    public async Task<ApiResponse<CustomerResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await context.Customers.Include(x => x.Orders).FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive==true, cancellationToken);
        if (customer == null)
        {
            return new ApiResponse<CustomerResponse>("Customer not found");
        }

        var mapped = mapper.Map<CustomerResponse>(customer);
        return new ApiResponse<CustomerResponse>(mapped);
    }

    public async Task<ApiResponse<List<OrderResponse>>> Handle(GetOrdersByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await context.Customers.Include(x => x.Orders).FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive==true, cancellationToken);
        if (customer == null)
        {
            return new ApiResponse<List<OrderResponse>>("Customer not found");
        }

        var mapped = mapper.Map<List<OrderResponse>>(customer.Orders);
        return new ApiResponse<List<OrderResponse>>(mapped);
    }
}