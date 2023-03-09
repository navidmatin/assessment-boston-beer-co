using Challenge3Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Challenge3Api.Tests
{
    public class Tests
    {
        private ProductsController _productsController;
        private IProductRepo _productRepo;

        [SetUp]
        public void Setup()
        {
            _productRepo = Substitute.For<IProductRepo>();
            _productsController = new ProductsController(Substitute.For<ILogger<ProductsController>>(), _productRepo);
        }

        [Test]
        public void GetProduct_ShouldFilter_IfNamePassedInt()
        {
            _productsController.GetProducts("test");
            _productRepo.Received(1).GetProducts("test");
        }

        [Test]
        public async Task CreateProduct_ShouldCreateProduct_IfProductIsValid()
        {
            var category = "test";
            var categoryId = 1;
            var product = new Models.Product { Category = category };
            var categoryModel = new Models.Category { Id = categoryId, Name = category };
            _productRepo.GetCategory(category).Returns(categoryModel);
         
            var result = await _productsController.CreateProduct(product);

            await _productRepo.Received(1).GetCategory(category);
            await _productRepo.Received(1).CreateProduct(product, categoryId);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task CreateProduct_ShouldReturnBadRequest_IfProductIsNotValid()
        {
            var category = "test";
            var product = new Models.Product { Category = category };

            var result = await _productsController.CreateProduct(product);

            await _productRepo.Received(1).GetCategory(category);
            await _productRepo.DidNotReceiveWithAnyArgs().CreateProduct(product, 1);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}