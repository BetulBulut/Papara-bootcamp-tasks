using ModelUse.Models;

namespace ModelUse.Services;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAll();
    Task<Book> GetById(int id);
    Task Add(Book book);
    Task Update(Book product);
    Task Delete(int id);
}