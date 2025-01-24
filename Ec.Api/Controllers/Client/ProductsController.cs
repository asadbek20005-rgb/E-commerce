using Ec.Data.Enums;
using Ec.Service.Api.Client;
using Microsoft.AspNetCore.Mvc;

namespace Ec.Api.Controllers.Client;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(ProductService clientProductService) : ControllerBase
{
    private readonly ProductService _clientProductService = clientProductService;

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _clientProductService.GetProducts();
        return Ok(products);
    }
    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetProductById(Guid productId)
    {
        var product = await _clientProductService.GetProductById(productId);
        return Ok(product);
    }

    [HttpGet("{category:enum}")]
    public async Task<IActionResult> GetProductsByCategory(Category category)
    {
        var products = await _clientProductService.GetProductsByCategory(category);
        return Ok(products);
    }

    [HttpGet("price")]
    public async Task<IActionResult> GetProductsByPrice(decimal price)
    {
        var producst = await _clientProductService.GetProductsByPrice(price);
        return Ok(producst);
    }
    [HttpGet("price-range")]
    public async Task<IActionResult> GetProductsByPriceRange(decimal firstPrice, decimal lastPrice)
    {
        var products = await _clientProductService.GetProductsByPriceRange(firstPrice, lastPrice);
        return Ok(products);
    }

    [HttpGet("{productId:guid}/video")]
    public async Task<IActionResult> GetVideo(Guid productId, string fileName)
    {
        var stream = await _clientProductService.GetVideo(productId, fileName);
        return Ok(stream);
    }

}