using Challenge3Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Challenge3Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductRepo _productRepo;

        public ProductsController(ILogger<ProductsController> logger, IProductRepo productRepo)
        {
            _logger = logger;
            _productRepo = productRepo;
        }

        [HttpGet]
        public IEnumerable<Product> GetProducts(string? name = null)
        {
            if(name == null)
            {
                return _productRepo.GetProducts();
            }
            else
            {
                return _productRepo.GetProducts(name);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody]Product product)
        {
            var category = await _productRepo.GetCategory(product.Category);
            if(category == null)
            {
                return BadRequest("Invalid Category.");
            }
            await _productRepo.CreateProduct(product, category.Id);
            return Ok();
        }

        [HttpPut("$id")]
        public async Task<Product> RenameProduct(int id, string name)
        {
            await _productRepo.RenameProduct(id, name);
            return await _productRepo.GetProduct(id);
        }

        [HttpGet("$id")]
        public Task<Product> Get(int id)
        {
            return _productRepo.GetProduct(id);
        }

        [HttpGet("categories")]
        public IEnumerable<Category> GetCategories()
        {
            return _productRepo.GetCategories();
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory(string name)
        {
            await _productRepo.CreateCategory(name);
            return Ok();
        }
    }
}