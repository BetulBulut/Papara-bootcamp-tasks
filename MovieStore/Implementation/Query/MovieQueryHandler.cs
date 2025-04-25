using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStore.Data;
using MovieStore.Implementation.Cqrs;
using MovieStore.Schema;


namespace MovieStore.Implementation.Query;

public class MovieQueryHandler :
IRequestHandler<GetAllMoviesQuery, ApiResponse<List<MovieResponse>>>,
IRequestHandler<GetMovieByIdQuery, ApiResponse<MovieResponse>>
{
    private readonly AppDbContext context;
    private readonly IMapper mapper;

    public MovieQueryHandler( AppDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<MovieResponse>>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var Movies = await context.Movies.Where(x => x.IsActive==true).ToListAsync(cancellationToken);
        if (Movies == null || Movies.Count == 0)
        {
            return new ApiResponse<List<MovieResponse>>("No Movies found");
        }
        var mapped = mapper.Map<List<MovieResponse>>(Movies);
        return new ApiResponse<List<MovieResponse>>(mapped);
    }

    public async Task<ApiResponse<MovieResponse>> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var Movie = await context.Movies.Include(x => x.Actors).FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive==true, cancellationToken);
        if (Movie == null)
        {
            return new ApiResponse<MovieResponse>("Movie not found");
        }

        var mapped = mapper.Map<MovieResponse>(Movie);
        return new ApiResponse<MovieResponse>(mapped);
    }

}