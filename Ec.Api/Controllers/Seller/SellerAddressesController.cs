using Ec.Service.Api.Seller;
using Microsoft.AspNetCore.Mvc;

namespace Ec.Api.Controllers.Seller;

[Route("api/Sellers/{sellerId:guid}/[controller]")]
[ApiController]
public class SellerAddressesController(AddressService addressService) : ControllerBase
{
    private readonly AddressService _addressService = addressService;

    [HttpPost]
    public async Task<IActionResult> Create(Guid sellerId, string address)
    {
        var addressDto = await _addressService.Create(sellerId, address);
        return Ok(addressDto);
    }
}
