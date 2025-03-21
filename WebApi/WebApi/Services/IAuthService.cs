using WebApi.Models;

namespace WebApi.Services;

public interface IAuthService
{
    User Authenticate(string username, string password);
}