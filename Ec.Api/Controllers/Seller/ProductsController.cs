using Ec.Common.Models.Product;
using Ec.Service.Api.Seller;
using Microsoft.AspNetCore.Mvc;

namespace Ec.Api.Controllers.Seller
{
    [Route("api/Sellers/{sellerId:guid}/[controller]")]
    [ApiController]
    public class ProductsController(ProductService productService) : ControllerBase
    {
        private readonly ProductService _productService = productService;
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Guid sellerId, CreateProductModel model)
        {
            var result = await _productService.Create(sellerId, model);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(Guid sellerId)
        {
            var products = await _productService.GetProducts(sellerId);
            return Ok(products);
        }

        [HttpGet("{productId:guid}")]
        public async Task<IActionResult> GetProduct(Guid sellerId, Guid productId)
        {
            var product = await _productService.GetProduct(sellerId, productId);
            return Ok(product);
        }
    }
}
