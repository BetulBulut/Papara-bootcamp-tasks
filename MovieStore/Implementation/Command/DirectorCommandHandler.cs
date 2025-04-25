using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStore.Data;
using MovieStore.Implementation.Cqrs;
using MovieStore.Models;
using MovieStore.Schema;


namespace MovieStore.Implementation.Command;

public class DirectorCommandHandler :
IRequestHandler<CreateDirectorCommand, ApiResponse<DirectorResponse>>,
IRequestHandler<UpdateDirectorCommand, ApiResponse>,
IRequestHandler<DeleteDirectorCommand,ApiResponse>
{
    private readonly AppDbContext context;
    private readonly IMapper mapper;

    public DirectorCommandHandler(AppDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<ApiResponse> Handle(DeleteDirectorCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Directors.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            return new ApiResponse("Director not found");

        if (!entity.IsActive)
            return new ApiResponse("Director is not active");

        var Movies = await context.Movies.ToListAsync();
        if (Movies.Any(o => o.DirectorId == entity.Id ))
            throw new Exception("Director cannot be deleted. It is referenced by movies.");

        entity.IsActive = false;

        await context.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
     public async Task<ApiResponse> Handle(UpdateDirectorCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Directors.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            return new ApiResponse("Director not found");

        if (!entity.IsActive)
            return new ApiResponse("Director is not active");

        entity.FirstName = request.Director.FirstName;
        entity.LastName = request.Director.LastName;

        await context.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse<DirectorResponse>> Handle(CreateDirectorCommand request, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<Director>(request.Director);
        var existingDirector = await context.Directors.FirstOrDefaultAsync(x => x.FirstName == mapped.FirstName && x.LastName == mapped.LastName, cancellationToken);
        if (existingDirector != null)
            return new ApiResponse<DirectorResponse>("Director already exists");
        mapped.IsActive = true;

        var entity = await context.AddAsync(mapped, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        var response = mapper.Map<DirectorResponse>(entity.Entity);
        return new ApiResponse<DirectorResponse>(response);
    }
}