using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.WebApi.Controllers
{
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IRepository<Product> _repository;

        public CatalogController(IRepository<Product> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        [HttpPost("add_product")]
        public async Task<ActionResult> AddProduct(Product product, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.Add(product, cancellationToken);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
        [HttpGet("get_product")]
        public async Task<ActionResult<Product>> GetProductById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _repository.GetById(id, cancellationToken);
                return Ok(product);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
        [HttpGet("get_products")]
        public async Task<ActionResult<List<Product>>> GetAllProducts(CancellationToken cancellationToken)
        {
            try
            {
                var products = await _repository.GetAll(cancellationToken);
                return Ok(products);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
        [HttpPost("update_product")]
        public async Task<ActionResult> UpdateProduct(Product product, CancellationToken cancellationToken)
        {
            await _repository.Update(product, cancellationToken);
            return Ok();
        }
        [HttpPost("delete_product")]
        public async Task<ActionResult> DeleteProduct([FromBody] Product product, CancellationToken cancellationToken)
        {
            await _repository.Delete(product, cancellationToken);
            return Ok();
        }
    }
}
