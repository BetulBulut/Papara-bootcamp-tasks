using MovieStore.Models;

namespace MovieStore.Schema;

public class ActorRequest : BaseRequest
{
   public string FirstName { get; set; }
   public string LastName { get; set; }
}

public class ActorResponse : BaseResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<MovieResponse> ActedMovies { get; set; }
}