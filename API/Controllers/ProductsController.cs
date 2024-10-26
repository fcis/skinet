using Core.Entities;
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
        private readonly StoreContext storeContext;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(StoreContext storeContext,ILogger<ProductsController> _logger)
        {
            this.storeContext = storeContext;
            this.logger = _logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(){

            return await storeContext.Products.ToListAsync();
        }
        [HttpGet("{id:int}")] // api/Products/3
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await storeContext.Products.FindAsync(id);
            if(product == null)
                return NotFound();
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            try{
                await storeContext.AddAsync(product);
                await storeContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetProduct),new {id = product.Id},product);
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
            storeContext.Entry(product).State = EntityState.Modified;
             await storeContext.SaveChangesAsync();
            return NoContent();
        }
         [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await storeContext.Products.FindAsync(id);
        if (product == null) return NotFound();
        storeContext.Products.Remove(product);
        await storeContext.SaveChangesAsync();
        return NoContent();
    }
       private bool ProductExists(int id)
        {
            return storeContext.Products.Any(x => x.Id == id);
        }
    }
}
