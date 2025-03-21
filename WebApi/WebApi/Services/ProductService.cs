using RestfulApiProject.Data;
using RestfulApiProject.Models;
using RestfulApiProject.Repositories;
using RestfulApiProject.Services;

namespace WebApi.Services;
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Add(Product product)
    {
        await _productRepository.Add(product);
    }

    public async Task Delete(int id)
    {
        await _productRepository.Delete(id);
    }

    public Task<IEnumerable<Product>> GetAll()
    {
        return _productRepository.GetAll();
    }

    public Task<Product> GetById(int id)
    {
        return _productRepository.GetById(id);
    }

    public Task Update(Product product)
    {
        return _productRepository.Update(product);
    }
}

