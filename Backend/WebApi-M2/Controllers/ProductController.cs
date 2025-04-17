using Microsoft.AspNetCore.Mvc;
using WebApi_M2.Models;
using WebApi_M2.Services;

namespace WebApi_M2.Controllers;

[ApiController]
[Route("[controller]")] 
public class ProductController : ControllerBase 
{
    private readonly ProductService _productService;
    public ProductController(ProductService productService) 
    {
        _productService = productService;
    }

    [HttpGet]
    public ActionResult<List<Product>> GetAllProducts() 
    {
        var products = _productService.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public ActionResult<Product> GetProductById(int id) 
    {
        var product = _productService.GetProductById(id);
        if (product == null) 
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public ActionResult<Product> AddProduct([FromBody] Product product) 
    {
        _productService.AddProduct(product);
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateProduct(int id, [FromBody] Product product) 
    {
        if (id != product.Id) 
        {
            return BadRequest();
        }
        var existingProduct = _productService.GetProductById(id);
        if (existingProduct == null) 
        {
            return NotFound();
        }
        _productService.UpdateProduct(product);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public ActionResult DeleteProduct(int id) 
    {
        var existingProduct = _productService.GetProductById(id);
        if (existingProduct == null) 
        {
            return NotFound();
        }
        _productService.DeleteProduct(id);
        return NoContent();
    }
}