using BookStore.Data;
using BookStore.Models;
using AutoMapper;

namespace BookStore.Application.AuthorOperations.Command.CreateAuthor;

public class CreateAuthorCommand
{
    public CreateAuthorModel Model { get; set; }
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateAuthorCommand(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public void Handle()
    {
        var Author = _dbContext.Authors.SingleOrDefault(b => b.FirstName == Model.FirstName && b.LastName == Model.LastName);
        if (Author is not null)
            throw new InvalidOperationException("Author already exists.");

        Author = _mapper.Map<Author>(Model);
        _dbContext.Authors.Add(Author);
        _dbContext.SaveChanges();
    }
}

public class CreateAuthorModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
}
