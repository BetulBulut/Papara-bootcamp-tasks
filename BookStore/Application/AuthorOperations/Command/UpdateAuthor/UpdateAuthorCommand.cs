using BookStore.Data;
using AutoMapper;

namespace BookStore.Application.AuthorOperations.Command;

public class UpdateAuthorCommand
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public int AuthorId { get; set; }
    public UpdateAuthorModel UpdatedAuthor { get; set; }

    public UpdateAuthorCommand(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public void Handle()
    {
        var Author = _context.Authors.SingleOrDefault(x => x.Id == AuthorId);
        if (Author == null)
            throw new InvalidOperationException("Author not found.");

        _mapper.Map(UpdatedAuthor, Author);
        _context.SaveChanges();
    }
}

public class UpdateAuthorModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
