using Ec.Common.Constants;
using Ec.Common.Models.Admin;
using Ec.Service.Api.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ec.Api.Controllers.Admin;

[Route("api/[controller]/action")]
[ApiController]
[Authorize(Roles = Constants.AdminRole)]
public class AdminsController(AdminService adminService) : ControllerBase
{
    private readonly AdminService _adminService = adminService;

    [HttpPost("login")]
    public async Task<IActionResult> Login(AdminLoginModel model)
    {
        string result = await _adminService.Login(model);
        return Ok(result);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _adminService.GetUsersAsync();
        return Ok(users);
    }
    [HttpGet("users/{userId:guid}")]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        var user = await _adminService.GetUserById(userId);
        return Ok(user);
    }


    [HttpGet("products/{productId:guid}")]
    public async Task<IActionResult> GetProductById(Guid productId)
    {
        var product = await _adminService.GetProductById(productId);
        return Ok(product);
    }
    [HttpGet("products")]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _adminService.GetProductsAsync();
        return Ok(products);
    }

    [HttpPut("blog-user")]
    public async Task<IActionResult> BlogUser(Guid userId)
    {
        bool result = await _adminService.BlogUser(userId);
        return Ok(result);
    }

    [HttpPut("unblog-user")]
    public async Task<IActionResult> UnblogUser(Guid userId)
    {
        bool result = await _adminService.UnblogUser(userId);
        return Ok(result);
    }

    [HttpDelete("users/{userId:guid}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        bool result = await _adminService.DeleteUser(userId);
        return Ok(result);
    }

    [HttpDelete("products/{productId:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid productId)
    {
        bool result = await _adminService.DeleteProduct(productId);
        return Ok(result);
    }
}
