using MovieStore.Models;

namespace MovieStore.Schema;

public class AuthRequest : BaseRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string CustomerId { get; set; }
    
}

public class AuthResponse : BaseResponse
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public string CustomerId { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}