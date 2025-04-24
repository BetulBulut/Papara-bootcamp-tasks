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
IRequestHandler<UpdateCustomerCommand,ApiResponse>,
IRequestHandler<DeleteCustomerCommand,ApiResponse>
{
    private readonly AppDbContext dbContext;
    private readonly IMapper mapper;

    public CustomerCommandHandler(AppDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<Customer>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            return new ApiResponse("Customer not found");

        if (!entity.IsActive)
            return new ApiResponse("Customer is not active");

        entity.IsActive = false;

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<Customer>().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            return new ApiResponse("Customer not found");

        if (!entity.IsActive)
            return new ApiResponse("Customer is not active");

        entity.FirstName = request.customer.FirstName;
        entity.LastName = request.customer.LastName;
        entity.Username = request.customer.Username;

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse<CustomerResponse>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<Customer>(request.customer);
        var existingCustomer = await dbContext.Set<Customer>().FirstOrDefaultAsync(x => x.Username == mapped.Username, cancellationToken);
        if (existingCustomer != null)
            return new ApiResponse<CustomerResponse>("Customer already exists");
        mapped.IsActive = true;

        //secret ve password üret

        mapped.Secret="secret";
        mapped.PasswordHash = "passwordHash";
        mapped.FavoriteGenres = new List<GenreEnum>(){GenreEnum.Action, GenreEnum.Comedy};
        

        var entity = await dbContext.AddAsync(mapped, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        var response = mapper.Map<CustomerResponse>(entity.Entity);

        return new ApiResponse<CustomerResponse>(response);
    }

    
}