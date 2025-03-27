using ModelUse.Models;
using ModelUse.Repositories;

namespace ModelUse.Services;
public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task Add(Book book)
    {
        await _bookRepository.AddAsync(book);
    }

    public async Task Delete(int id)
    {
        await _bookRepository.DeleteAsync(id);
    }

    public Task<IEnumerable<Book>> GetAll()
    {
        return _bookRepository.GetAllAsync();
    }

    public Task<Book> GetById(int id)
    {
        return _bookRepository.GetByIdAsync(id);
    }

    public Task Update(Book book)
    {
        return _bookRepository.UpdateAsync(book);
    }
}

