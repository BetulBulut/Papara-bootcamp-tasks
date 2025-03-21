using Azure.Identity;
using WebApi.Models;

namespace WebApi.Services;

public class AuthService : IAuthService
{
    private readonly List<User> _users = new()
    {
        new User { Id = 1, Username = "admin", Password = "123456", Role = "Admin" },
        new User { Id = 2, Username = "user", Password = "password", Role = "User" }
    };

   
    public User Authenticate(string username, string password)
    {

        var user = _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        
        return user;
    }

}