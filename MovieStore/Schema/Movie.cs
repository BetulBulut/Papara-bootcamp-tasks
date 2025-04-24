using MovieStore.Models;

namespace MovieStore.Schema;

public class MovieRequest : BaseRequest
{
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
    public GenreEnum Genre { get; set; }
    public decimal Price { get; set; }
    public int DirectorId { get; set; }
    public List<int> ActorIds { get; set; }
}

public class MovieResponse : BaseResponse
{
    public string Title { get; set; }
    public int ReleaseYear { get; set; }
    public GenreEnum Genre { get; set; }
    public decimal Price { get; set; }
    public DirectorResponse Director { get; set; }
    public List<ActorResponse> Actors { get; set; }
}
