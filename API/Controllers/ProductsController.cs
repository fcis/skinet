using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository repo;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(IProductRepository Repo,ILogger<ProductsController> _logger)
        {
            repo = Repo;
            this.logger = _logger;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, 
        string? type, string? sort)
         {
                return Ok(await repo.GetProductsAsync(brand, type, sort));
        }
        [HttpGet("{id:int}")] // api/Products/3
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await repo.GetProductByIdAsync(id);
            if(product == null)
                return NotFound();
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            try{
                 repo.AddProduct(product);
               if(await repo.SaveChangesAsync())
               {
                 return CreatedAtAction(nameof(GetProduct),new {id = product.Id},product);
               }
               return BadRequest("Problem Creating Product");
            }
            catch(Exception e)
            {
             logger.LogError(e,"log");
             return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id,[FromBody] Product product)
        {
            if (product.Id != id || !repo.ProductExists(id)) 
            return BadRequest("Cannot update this product");
            repo.UpdateProduct(product);
             if(await repo.SaveChangesAsync())
            return NoContent();
            else
            return BadRequest("Problem Updating Product");
        }
         [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);
        if (product == null) return NotFound();
            repo.DeleteProduct(product);
        if(await repo.SaveChangesAsync())
            return NoContent();
        else
            return BadRequest("Problem Updating Product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await repo.GetBrandsAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await repo.GetTypesAsync());
    }

    }
}
