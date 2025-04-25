using MovieStore.Models;

namespace MovieStore.Schema;

public class LoginRequest : BaseRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string CustomerId { get; set; }
    
}

public class LoginResponse : BaseResponse
{
    public string Token { get; set; }
    public string UserName { get; set; }
    public DateTime Expiration { get; set; }
}