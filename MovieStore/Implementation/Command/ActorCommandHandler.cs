using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStore.Data;
using MovieStore.Implementation.Cqrs;
using MovieStore.Models;
using MovieStore.Schema;


namespace MovieStore.Implementation.Command;

public class ActorCommandHandler :
IRequestHandler<CreateActorCommand, ApiResponse<ActorResponse>>,
IRequestHandler<UpdateActorCommand, ApiResponse>,
IRequestHandler<DeleteActorCommand,ApiResponse>
{
    private readonly AppDbContext context;
    private readonly IMapper mapper;

    public ActorCommandHandler(AppDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<ApiResponse> Handle(DeleteActorCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Actors.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            return new ApiResponse("Actor not found");

        if (!entity.IsActive)
            return new ApiResponse("Actor is not active");

        var Movies = await context.Movies.ToListAsync();
        if (Movies.Any(o => o.Actors.Any(a => a.Id == entity.Id)))
            throw new Exception("Actor cannot be deleted. It is referenced by movies.");

        entity.IsActive = false;

        await context.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
     public async Task<ApiResponse> Handle(UpdateActorCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Actors.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            return new ApiResponse("Actor not found");

        if (!entity.IsActive)
            return new ApiResponse("Actor is not active");

        entity.FirstName = request.Actor.FirstName;
        entity.LastName = request.Actor.LastName;

        await context.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse<ActorResponse>> Handle(CreateActorCommand request, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<Actor>(request.Actor);
        var existingActor = await context.Actors.FirstOrDefaultAsync(x => x.FirstName == mapped.FirstName && x.LastName == mapped.LastName, cancellationToken);
        if (existingActor != null)
            return new ApiResponse<ActorResponse>("Actor already exists");
        mapped.IsActive = true;

        var entity = await context.AddAsync(mapped, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        var response = mapper.Map<ActorResponse>(entity.Entity);
        return new ApiResponse<ActorResponse>(response);
    }
}