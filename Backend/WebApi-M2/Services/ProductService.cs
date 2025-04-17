
using WebApi_M2.Models;

namespace WebApi_M2.Services;

public class ProductService 
{
    private List<Product> _products = new List<Product>();

    public List<Product> GetAllProducts()
    {
        return _products;
    }

    public Product GetProductById(int id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }

    public void AddProduct(Product product)
    {
        product.Id = GetNextId();
        _products.Add(product);
    }

    public void UpdateProduct(Product product)
    {
        var existingProduct = GetProductById(product.Id);
        if (existingProduct != null)
        {
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
        }
    }

    public void DeleteProduct(int id)
    {
        var productToRemove = GetProductById(id);
        if (productToRemove != null)
        {
            _products.Remove(productToRemove);
        }
    }

    private int GetNextId()
    {
        return _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
    }
}