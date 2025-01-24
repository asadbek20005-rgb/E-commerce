﻿using Ec.Common.Models.Admin;
using Ec.Service.Api.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Ec.Api.Controllers.Admin;

[Route("api/[controller]/action")]
[ApiController]
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
}
