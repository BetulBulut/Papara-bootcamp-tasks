using MovieStore.Helpers;
using MediatR;
using MovieStore.Data;
using MovieStore.Implementation.Cqrs;
using MovieStore.Schema;
using MovieStore.Services;


namespace MovieStore.Implementation.Query;

public class AuthQueryHandler :
IRequestHandler<LoginQuery, ApiResponse<LoginResponse>>
{
    private readonly AppDbContext context;
    private readonly ITokenService tokenService;
    public AuthQueryHandler( AppDbContext context, ITokenService tokenService)
    {
        this.context = context;
        this.tokenService = tokenService;
    }
    
    public async Task<ApiResponse<LoginResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = context.Customers.FirstOrDefault(x => x.Username == request.LoginRequest.Username);
        if (user == null)
        {
            return new ApiResponse<LoginResponse>("User name or password is incorrect");
        }
        var hashedPassword = PasswordGenerator.CreateMD5(request.LoginRequest.Password, user.Secret);
        if (hashedPassword != user.PasswordHash)
            return new ApiResponse<LoginResponse>("User name or password is incorrect");

        var token = tokenService.GenerateToken(user);
        var response = new LoginResponse
        {
            UserName = user.Username,
            Token = token,
            Expiration = DateTime.UtcNow.AddMinutes(55)
        };
        return new ApiResponse<LoginResponse>(response);
    }
}