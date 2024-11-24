using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Specifications;
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
        private readonly IGenericRepository<Product> repo;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(IGenericRepository<Product> Repo,ILogger<ProductsController> _logger)
        {
            repo = Repo;
            this.logger = _logger;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, 
        string? type, string? sort)
        {
            var spec= new ProductSpecification(brand,type,sort);
            var products = await repo.ListAsync(spec);
                return Ok(products);
        }
        [HttpGet("{id:int}")] // api/Products/3
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await repo.GetByIdAsync(id);
            if(product == null)
                return NotFound();
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            try{
                 repo.Add(product);
               if(await repo.SaveAllAsync())
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
            if (product.Id != id || !repo.Exists(id)) 
            return BadRequest("Cannot update this product");
            repo.Update(product);
             if(await repo.SaveAllAsync())
            return NoContent();
            else
            return BadRequest("Problem Updating Product");
        }
         [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);
        if (product == null) return NotFound();
            repo.Remove(product);
        if(await repo.SaveAllAsync())
            return NoContent();
        else
            return BadRequest("Problem Updating Product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();
         var brands = await repo.ListAsync(spec);
        return Ok(brands);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();
        var types = await repo.ListAsync(spec);

        return Ok(types);
    }

    }
}
