using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStore.Data;
using MovieStore.Implementation.Cqrs;
using MovieStore.Schema;


namespace MovieStore.Implementation.Query;

public class DirectorQueryHandler :
IRequestHandler<GetAllDirectorsQuery, ApiResponse<List<DirectorResponse>>>,
IRequestHandler<GetDirectorByIdQuery, ApiResponse<DirectorResponse>>
{
    private readonly AppDbContext context;
    private readonly IMapper mapper;

    public DirectorQueryHandler( AppDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<DirectorResponse>>> Handle(GetAllDirectorsQuery request, CancellationToken cancellationToken)
    {
        var Directors = await context.Directors.Where(x => x.IsActive==true).ToListAsync(cancellationToken);
        if (Directors == null || Directors.Count == 0)
        {
            return new ApiResponse<List<DirectorResponse>>("No Directors found");
        }
        var mapped = mapper.Map<List<DirectorResponse>>(Directors);
        return new ApiResponse<List<DirectorResponse>>(mapped);
    }

    public async Task<ApiResponse<DirectorResponse>> Handle(GetDirectorByIdQuery request, CancellationToken cancellationToken)
    {
        var Director = await context.Directors.Include(x => x.DirectedMovies).FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive==true, cancellationToken);
        if (Director == null)
        {
            return new ApiResponse<DirectorResponse>("Director not found");
        }

        var mapped = mapper.Map<DirectorResponse>(Director);
        return new ApiResponse<DirectorResponse>(mapped);
    }
}