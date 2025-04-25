using MediatR;
using MovieStore.Schema;


namespace MovieStore.Implementation.Cqrs;

public record LoginQuery(LoginRequest LoginRequest): IRequest<ApiResponse<LoginResponse>>;