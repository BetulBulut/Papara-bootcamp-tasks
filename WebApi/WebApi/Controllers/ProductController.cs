using Microsoft.AspNetCore.Mvc;
using RestfulApiProject.Models;
using RestfulApiProject.Services;
using WebApi.Authentication;
using WebApi.Services;

namespace RestfulApiProject.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;
    private readonly IAuthService _authService;


    public ProductController(IProductService service, IAuthService authService)
    {
        _service = service;
        _authService = authService;
    }

    // GET: api/products
    // Get all products or search by name
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] string? name)
    {
        var products = await _service.GetAll();

        if (!string.IsNullOrEmpty(name))
        {
            products = products.Where(p => p.Name.Contains(name)).ToList();
        }

        return Ok(products);
    }

    // GET: api/products/{id}
    // Get product by id
    [HttpGet("{id}")]
    [Auth]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _service.GetById(id);
        if (product == null)
            return NotFound(new { message = "Product not found" });

        return Ok(product);
    }

    // POST: api/products
    // Create a new product
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        product.Id = null;
        await _service.Add(product);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    // Update prduct
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        var existingProduct = await _service.GetById(id); 
        if (existingProduct == null)
            return NotFound(new { message = "Product not found" });

        
        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;

    
        await _service.Update(existingProduct); 

        return NoContent(); // 204 No Content döndür
    }

    // Delete product
    // DELETE: api/products/{id}
    [HttpDelete("{id}")]
    [Auth]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product =await _service.GetById(id);
        if (product == null)
            return NotFound(new { message = "Product not found" });

        await _service.Delete(id);
        return NoContent();
    }

    // PATCH: api/products/{id}
    // Update product price
    [HttpPatch("{id}")]
    [Auth]
    public async Task<IActionResult> UpdateProductPrice(int id, [FromBody] decimal price)
    {
        var product = await _service.GetById(id);
        if (product == null)
            return NotFound(new { message = "Product not found" });
    
        product.Price = price;
        await _service.Update(product);
        return NoContent();
    }

    // GET: api/products/list
    // Get all products or search by name and sort by price or name
    [HttpGet("list")]
    [Auth]
    public async Task<ActionResult<IEnumerable<Product>>> GetSortedProductsAsync(
        [FromQuery] string? name, 
        [FromQuery] string sortBy)
    {
        var products = await _service.GetAll();

        if (!string.IsNullOrEmpty(name))
        {
            products = products.Where(p => p.Name.Contains(name)).ToList();
        }

        // Sıralama ekleme
        products = sortBy switch
        {
            "price" => products.OrderBy(p => p.Price).ToList(),
            "name" => products.OrderBy(p => p.Name).ToList(),
            _ => products
        };

        return Ok(products);
    }

}
