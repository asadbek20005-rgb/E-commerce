using Ec.Common.Constants;
using Ec.Service.Api.Seller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ec.Api.Controllers.Seller;

[Route("api/Sellers/{sellerId:guid}/[controller]")]
[ApiController]
[Authorize(Roles = Constants.SellerRole)]
public class SellerAddressesController(AddressService addressService) : ControllerBase
{
    private readonly AddressService _addressService = addressService;

    [HttpPost]
    public async Task<IActionResult> AddOrUpdateAddress(Guid sellerId, [FromQuery] string address)
    {
        var addressDto = await _addressService.AddOrUpdateAddress(sellerId, address);
        return Ok(addressDto);
    }
    [HttpGet]
    public async Task<IActionResult> GetAddressBySellerId(Guid sellerId)
    {
        var address = await _addressService.GetAddressBySellerId(sellerId);
        return Ok(address);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAddress(Guid sellerId)
    {
        bool result = await _addressService.DeleteAddress(sellerId);
        return Ok(result);
    }

}
