using Ec.Common.Models.Otp;
using Ec.Common.Models.Seller;
using Ec.Service.Api;
using Microsoft.AspNetCore.Mvc;

namespace Ec.Api.Controllers.Seller;

[Route("api/[controller]")]
[ApiController]
public class SellersController(SellerService sellerService) : ControllerBase
{
    private readonly SellerService _sellerService = sellerService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(SellerRegisterModel model)
    {
        int code = await _sellerService.Register(model);
        return Ok(code);
    }

    [HttpPost("verify-register")]
    public async Task<IActionResult> VerifyRegister(OtpModel otpModel)
    {
        string result = await _sellerService.VerifyRegister(otpModel);
        return Ok(result);
    }

    [HttpPatch("login")]
    public async Task<IActionResult> Login(SellerLoginModel model)
    {
        int code = await _sellerService.Login(model);
        return Ok(code);
    }
}
