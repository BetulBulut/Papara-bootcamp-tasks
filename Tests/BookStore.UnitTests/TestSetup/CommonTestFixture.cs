using BookStore.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BookStore.Mappings;

namespace TestSetup;
public class CommonTestFixture
{
    public AppDbContext Context { get; set; }
    public IMapper Mapper { get; set; }


public CommonTestFixture()
{
    var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "BookStoreTestDB").Options;
    Context = new AppDbContext(options);
    Context.Database.EnsureCreated();
    Context.AddGenres();
    Context.AddAuthors();
    Context.AddBooks();
    Context.SaveChanges();
    var config = new MapperConfiguration(cfg =>
    {
        cfg.AddProfile<MappingProfile>();
    });
    Mapper = config.CreateMapper();
}
}