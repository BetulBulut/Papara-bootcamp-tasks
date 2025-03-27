using ModelUse.Models;

namespace ModelUse.Repositories;

public interface IBookRepository
{
    Task<Book> GetByIdAsync(int id);
    Task<IEnumerable<Book>> GetAllAsync();
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(int id);
}
