using MovieStore.Models;

namespace MovieStore.Schema;

public class DirectorRequest : BaseRequest
{
   public string FirstName { get; set; }
   public string LastName { get; set; }
}

public class DirectorResponse : BaseResponse
{
   public string FirstName { get; set; }
   public string LastName { get; set; }
    public List<MovieResponse> DirectedMovies { get; set; }
}