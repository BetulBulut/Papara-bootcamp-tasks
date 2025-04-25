using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStore.Data;
using MovieStore.Implementation.Cqrs;
using MovieStore.Models;
using MovieStore.Schema;


namespace MovieStore.Implementation.Command;

public class MovieCommandHandler :
IRequestHandler<CreateMovieCommand, ApiResponse<MovieResponse>>,
IRequestHandler<DeleteMovieCommand,ApiResponse>,
IRequestHandler<UpdateMovieCommand, ApiResponse>
{
    private readonly AppDbContext context;
    private readonly IMapper mapper;

    public MovieCommandHandler(AppDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<ApiResponse> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Movies.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            return new ApiResponse("Movie not found");

        if (!entity.IsActive)
            return new ApiResponse("Movie is not active");

        var orders = await context.Orders.ToListAsync();
        if (orders.Any(o => o.MovieId == entity.Id))
            throw new Exception("Movie cannot be deleted. It is referenced by orders.");

        entity.IsActive = false;

        await context.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    
    public async Task<ApiResponse<MovieResponse>> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<Movie>(request.Movie);
        var existingMovie = await context.Movies.FirstOrDefaultAsync(x => x.Title == mapped.Title, cancellationToken);
        if (existingMovie != null)
            return new ApiResponse<MovieResponse>("Movie already exists");
        mapped.IsActive = true;
        mapped.DirectorId = request.Movie.DirectorId;
        if (context.Directors.FirstOrDefault(x => x.Id == request.Movie.DirectorId) == null)
            return new ApiResponse<MovieResponse>("Director not found");

        var entity = await context.AddAsync(mapped, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        var response = mapper.Map<MovieResponse>(entity.Entity);

        return new ApiResponse<MovieResponse>(response);
    }

    public async Task<ApiResponse> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Movies.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity == null)
            return new ApiResponse("Movie not found");

        if (!entity.IsActive)
            return new ApiResponse("Movie is not active");

        entity.Title = request.Movie.Title;
        entity.Genre = request.Movie.Genre;
        entity.Price = request.Movie.Price;

        await context.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}