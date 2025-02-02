using Ec.Common.Models.Product;
using Ec.Data.Enums;
using Ec.Service.Api.Seller;
using Microsoft.AspNetCore.Mvc;

namespace Ec.Api.Controllers.Seller
{
    [Route("api/Sellers/{sellerId:guid}/[controller]")]
    [ApiController]
    public class SellerProductsController(SellerProductService productService) : ControllerBase
    {
        private readonly SellerProductService _productService = productService;
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Guid sellerId, [FromBody] CreateProductModel model,Category category, ProductStatus status )
        {
            var result = await _productService.Create(sellerId, model, category, status);
            return Ok(result);
        }
        [HttpPost("{productId:guid}/file")]
        public async Task<IActionResult> UploadVideo(Guid sellerId, Guid productId, IFormFile file, string caption)
        {
            string videoUrl = await _productService.UploadFile(sellerId, productId, file, caption);
            return Ok(videoUrl);
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

        [HttpGet("{productId:guid}/file")]
        public async Task<IActionResult> GetVideo(Guid sellerId, Guid productId, [FromQuery] string fileName)
        {
            var ms = await _productService.GetFileAsync(sellerId, productId,fileName);
            return Ok(ms);
        }
        [HttpGet("by-price")]
        public async Task<IActionResult> GetProductsByPrice(Guid sellerId, [FromQuery] decimal price)
        {
            var products = await _productService.GetProductsByPrice(sellerId, price);
            return Ok(products);
        }

        [HttpGet("by-price-range")]
        public async Task<IActionResult> GetProductsByPriceRange(Guid sellerId, [FromQuery] decimal? startedPrice, [FromQuery] decimal? endedPrice)
        {
            var products = await _productService.GetProductsByPriceRange(sellerId, startedPrice, endedPrice);
            return Ok(products);
        }

        [HttpGet("date")]
        public async Task<IActionResult> GetProductsByDate(Guid sellerId, [FromQuery] string date)
        {
            var products = await _productService.GetProductsByDate(sellerId, date);
            return Ok(products);
        }

        [HttpPatch("{productId:guid}/price")]
        public async Task<IActionResult> UpdateProductPrice(Guid sellerId, Guid productId, decimal price)
        {
            bool result = await _productService.UpdateProductByPrice(sellerId, productId, price);
            return Ok(result);
        }
        [HttpPatch("{productId:guid}/description")]
        public async Task<IActionResult> UpdateProductDescrption(Guid sellerId, Guid productId, string description)
        {
            bool result = await _productService.UpdateProductByDescription(sellerId, productId, description);
            return Ok(result);
        }
        [HttpPatch("{productId:guid}/status")]
        public async Task<IActionResult> UpdateProductStatus(Guid sellerId, Guid productId, ProductStatus status)
        {
            bool result = await _productService.UpdateProductByStatus(sellerId, productId, status);
            return Ok(result);
        }
        [HttpPut("{productId:guid}")]
        public async Task<IActionResult> UpdateProduct(Guid sellerId, Guid productId, UpdateProductModel model)
        {
            bool result = await _productService.UpdateProduct(sellerId, productId, model);
            return Ok(result);
        }
        [HttpDelete("{productId:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid sellerId, Guid productId)
        {
            bool result = await _productService.DeleteProduct(sellerId, productId);
            return Ok(result);
        }
    }
}
