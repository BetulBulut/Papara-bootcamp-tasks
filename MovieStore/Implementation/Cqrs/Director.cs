using MediatR;
using MovieStore.Schema;


namespace MovieStore.Implementation.Cqrs;

public record GetAllDirectorsQuery :IRequest<ApiResponse<List<DirectorResponse>>>;
public record GetDirectorByIdQuery(int Id) : IRequest<ApiResponse<DirectorResponse>>;
public record CreateDirectorCommand(DirectorRequest Director) : IRequest<ApiResponse<DirectorResponse>>;
public record UpdateDirectorCommand(int Id,DirectorRequest Director) : IRequest<ApiResponse>;
public record DeleteDirectorCommand(int Id) : IRequest<ApiResponse>;