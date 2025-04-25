using MovieStore.Models;

namespace MovieStore.Services;
public interface ITokenService
{
    string GenerateToken(Customer customer);
}
