using Ec.Common.Models.Otp;
using Ec.Common.Models.Seller;
using Ec.Service.Api.Seller;
using Microsoft.AspNetCore.Mvc;

namespace Ec.Api.Controllers.Seller;

[Route("api/[controller]/account")]
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
        var sellerDto = await _sellerService.VerifyRegister(otpModel);
        return Ok(sellerDto);
    }

    [HttpPatch("login")]
    public async Task<IActionResult> Login(SellerLoginModel model)
    {
        int code = await _sellerService.Login(model);
        return Ok(code);
    }

    [HttpPost("verify-login")]
    public async Task<IActionResult> VerifyLogin(OtpModel model)
    {
        string result = await _sellerService.VerifyLogin(model);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAccount(Guid sellerId)
    {
        var sellerDto = await _sellerService.GetSellerAccount(sellerId);
        return Ok(sellerDto);
    }
}
