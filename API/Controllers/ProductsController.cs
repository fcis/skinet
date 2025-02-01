using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class ProductsController : BaseApiController 
    {
        private readonly IUnitOfWork unit;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(IUnitOfWork unit,ILogger<ProductsController> _logger)
        {
            this.unit = unit;
            this.logger = _logger;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProductSpecParams specParams)
        {
            
            var spec= new ProductSpecification(specParams);
            // var products = await repo.ListAsync(spec);
            // var count = await repo.CountAsync(spec);
            // var pagination = new Pagination<Product>(specParams.PageIndex,specParams.PageSize,count,products);
                return await CreatePagedResult(unit.Repository<Product>(),spec,specParams.PageIndex,specParams.PageSize);
        }
        [HttpGet("{id:int}")] // api/Products/3
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await unit.Repository<Product>().GetByIdAsync(id);
            if(product == null)
                return NotFound();
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            try{
                unit.Repository<Product>().Add(product);
               if(await unit.Complete())
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
            if (product.Id != id || !ProductExists(id)) 
            return BadRequest("Cannot update this product");
            unit.Repository<Product>().Update(product);
             if(await unit.Complete())
            return NoContent();
            else
            return BadRequest("Problem Updating Product");
        }
         [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await unit.Repository<Product>().GetByIdAsync(id);
        if (product == null) return NotFound();
            unit.Repository<Product>().Remove(product);
        if(await unit.Complete())
            return NoContent();
        else
            return BadRequest("Problem Updating Product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();
         var brands = await unit.Repository<Product>().ListAsync(spec);
        return Ok(brands);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();
        var types = await unit.Repository<Product>().ListAsync(spec);

        return Ok(types);
    }
     private bool ProductExists(int id)
     {
            return unit.Repository<Product>().Exists(id);
     }

    }
}
