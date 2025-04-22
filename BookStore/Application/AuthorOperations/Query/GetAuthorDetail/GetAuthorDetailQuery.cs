using BookStore.Data;
using AutoMapper;
using BookStore.Application.BookOperations.Query.GetBookDetail;

namespace BookStore.Application.AuthorOperations.Query;

public class GetAuthorDetailQuery
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    public int AuthorId { get; set; }

    public GetAuthorDetailQuery(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public AuthorDetailViewModel Handle()
    {
        var Author = _dbContext.Authors.SingleOrDefault(Author => Author.Id == AuthorId);
        if (Author is null)
            throw new InvalidOperationException("Author not found.");

        var result = _mapper.Map<AuthorDetailViewModel>(Author); 
        return result;
    }
}

public class AuthorDetailViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string BirthDate { get; set; }

    public List<BookDetailViewModel> Books { get; set; } = new List<BookDetailViewModel>();
    
}
