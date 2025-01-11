using Ec.Common.Models.Admin;
using Ec.Service.Api.SuperAdmin;
using Microsoft.AspNetCore.Mvc;

namespace Ec.Api.Controllers.SuperAdmin;

[Route("api/[controller]/action")]
[ApiController]
public class SuperAdminController(SuperAdminService superAdminService) : ControllerBase
{
    private readonly SuperAdminService _superAdminService = superAdminService;

    [HttpPost("admin")]
    public async Task<IActionResult> CreateAdmin(AdminCreateModel model)
    {
        var (username, password) = await _superAdminService.CreateAdmin(model);
        return Ok(model);
    }

    [HttpGet("admins")]
    public async Task<IActionResult> GetAdmins()
    {
        var admins = await _superAdminService.GetAdminsAsync();
        return Ok(admins);
    }
}
