using MovieStore.Models;

namespace MovieStore.Schema;

public class CustomerRequest : BaseRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
}

public class CustomerResponse : BaseResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public List<GenreEnum> FavoriteGenres { get; set; }
    public List<OrderResponse> Orders { get; set; }
}

