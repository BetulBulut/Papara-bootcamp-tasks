using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStore.Data;
using MovieStore.Implementation.Cqrs;
using MovieStore.Schema;


namespace MovieStore.Implementation.Query;

public class ActorQueryHandler :
IRequestHandler<GetAllActorsQuery, ApiResponse<List<ActorResponse>>>,
IRequestHandler<GetActorByIdQuery, ApiResponse<ActorResponse>>
{
    private readonly AppDbContext context;
    private readonly IMapper mapper;

    public ActorQueryHandler( AppDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<ActorResponse>>> Handle(GetAllActorsQuery request, CancellationToken cancellationToken)
    {
        var Actors = await context.Actors.Where(x => x.IsActive==true).ToListAsync(cancellationToken);
        if (Actors == null || Actors.Count == 0)
        {
            return new ApiResponse<List<ActorResponse>>("No Actors found");
        }
        var mapped = mapper.Map<List<ActorResponse>>(Actors);
        return new ApiResponse<List<ActorResponse>>(mapped);
    }

    public async Task<ApiResponse<ActorResponse>> Handle(GetActorByIdQuery request, CancellationToken cancellationToken)
    {
        var Actor = await context.Actors.Include(x => x.ActedMovies).FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive==true, cancellationToken);
        if (Actor == null)
        {
            return new ApiResponse<ActorResponse>("Actor not found");
        }

        var mapped = mapper.Map<ActorResponse>(Actor);
        return new ApiResponse<ActorResponse>(mapped);
    }
}