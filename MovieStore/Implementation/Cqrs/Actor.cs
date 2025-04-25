using MediatR;
using MovieStore.Schema;


namespace MovieStore.Implementation.Cqrs;

public record GetAllActorsQuery :IRequest<ApiResponse<List<ActorResponse>>>;
public record GetActorByIdQuery(int Id) : IRequest<ApiResponse<ActorResponse>>;
public record CreateActorCommand(ActorRequest Actor) : IRequest<ApiResponse<ActorResponse>>;
public record UpdateActorCommand(int Id,ActorRequest Actor) : IRequest<ApiResponse>;
public record DeleteActorCommand(int Id) : IRequest<ApiResponse>;