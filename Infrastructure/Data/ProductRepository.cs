using System;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class ProductRepository:IProductRepository
{
    private readonly StoreContext context;

    public ProductRepository(StoreContext storeContext)
    {
        this.context = storeContext;
    }

    public void AddProduct(Product product)
    {
        throw new NotImplementedException();
    }

    public void DeleteProduct(Product product)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Product?> GetProductByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<string>> GetTypesAsync()
    {
        throw new NotImplementedException();
    }

    public bool ProductExists(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    public void UpdateProduct(Product product)
    {
        throw new NotImplementedException();
    }
}
