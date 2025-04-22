using BookStore.Data;
using AutoMapper;

namespace BookStore.Application.AuthorOperations.Query;

public class GetAuthorsQuery
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAuthorsQuery(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public List<AuthorsViewModel> Handle()
    {
        var AuthorList = _dbContext.Authors.OrderBy(x => x.Id).ToList();
        var result = _mapper.Map<List<AuthorsViewModel>>(AuthorList);
        return result;
    }

}

public class AuthorsViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }

}

