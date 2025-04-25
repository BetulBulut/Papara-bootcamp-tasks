using MediatR;
using MovieStore.Schema;


namespace MovieStore.Implementation.Cqrs;

public record GetAllMoviesQuery :IRequest<ApiResponse<List<MovieResponse>>>;
public record GetMovieByIdQuery(int Id) : IRequest<ApiResponse<MovieResponse>>;
public record CreateMovieCommand(MovieRequest Movie) : IRequest<ApiResponse<MovieResponse>>;
public record UpdateMovieCommand(int Id,MovieRequest Movie) : IRequest<ApiResponse>;
public record DeleteMovieCommand(int Id) : IRequest<ApiResponse>;