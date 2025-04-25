using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStore.Data;
using MovieStore.Implementation.Cqrs;
using MovieStore.Models;
using MovieStore.Schema;


namespace MovieStore.Implementation.Command;

public class CustomerCommandHandler :
IRequestHandler<CreateCustomerCommand, ApiResponse<CustomerResponse>>,
IRequestHandler<DeleteCustomerCommand,ApiResponse>,
IRequestHandler<CreateFavoriteGenreCommand, ApiResponse>,
IRequestHandler<AddOrderCommand, ApiResponse<OrderResponse>>
{
    private readonly AppDbContext context;
    private readonly IMapper mapper;

    public CustomerCommandHandler(AppDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<ApiResponse> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Set<Customer>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            return new ApiResponse("Customer not found");

        if (!entity.IsActive)
            return new ApiResponse("Customer is not active");

        var orders = await context.Orders.ToListAsync();
        if (orders.Any(o => o.CustomerId == entity.Id))
            throw new Exception("Customer cannot be deleted. It is referenced by orders.");

        entity.IsActive = false;

        await context.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    
    public async Task<ApiResponse<CustomerResponse>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<Customer>(request.customer);
        var existingCustomer = await context.Set<Customer>().FirstOrDefaultAsync(x => x.Username == mapped.Username, cancellationToken);
        if (existingCustomer != null)
            return new ApiResponse<CustomerResponse>("Customer already exists");
        mapped.IsActive = true;

        //secret ve password üret

        mapped.Secret="secret";
        mapped.PasswordHash = "passwordHash";
        mapped.FavoriteGenres = new List<GenreEnum>(){GenreEnum.Action, GenreEnum.Comedy};
        

        var entity = await context.AddAsync(mapped, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        var response = mapper.Map<CustomerResponse>(entity.Entity);

        return new ApiResponse<CustomerResponse>(response);
    }

    public async Task<ApiResponse> Handle(CreateFavoriteGenreCommand request, CancellationToken cancellationToken)
    {
        var customer = await context.Set<Customer>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (customer == null)
            return new ApiResponse("Customer not found");

        if (!customer.IsActive)
            return new ApiResponse("Customer is not active");

        if (!Enum.TryParse<GenreEnum>(request.GenreId.ToString(), out var genre))
            return new ApiResponse("Invalid genre ID");

        customer.FavoriteGenres.Add(genre);
        await context.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse<OrderResponse>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await context.Set<Customer>().FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken);
        if (customer == null)
            return new ApiResponse<OrderResponse>("Customer not found");

        if (!customer.IsActive)
            return new ApiResponse<OrderResponse>("Customer is not active");

        var order = new Order
        {
            MovieId = request.MovieId,
            CustomerId = request.CustomerId,
            PurchaseDate = DateTime.UtcNow,
            IsActive = true,
        };

        await context.Orders.AddAsync(order, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var response = mapper.Map<OrderResponse>(order);
        return new ApiResponse<OrderResponse>(response);
    }
}