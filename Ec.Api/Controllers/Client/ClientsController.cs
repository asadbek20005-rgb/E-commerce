using Ec.Common.Constants;
using Ec.Common.Models.Client;
using Ec.Common.Models.Otp;
using Ec.Service.Api.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ec.Api.Controllers.Client
{
    [Route("api/[controller]/account")]
    [ApiController]
    public class ClientsController(ClientService clientService) : ControllerBase
    {
        private readonly ClientService _clientService = clientService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(ClientRegisterModel clientRegisterModel)
        {
            int code = await _clientService.Register(clientRegisterModel);
            return Ok(code);
        }
        [HttpPost("verify-register")]
        public async Task<IActionResult> VerifyRegsiter(OtpModel model)
        {
            var clientDto = await _clientService.VerifyRegister(model);
            return Ok(clientDto);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(ClientLoginModel model)
        {
            int code = await _clientService.Login(model);
            return Ok(code);
        }
        [HttpPost("verify-login")]
        public async Task<IActionResult> VerifyLogin(OtpModel model)
        {
            string result = await _clientService.VerifyLogin(model);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Constants.ClientRole)]
        public async Task<IActionResult> GetAccount(Guid clientId)
        {
            var clientDto = await _clientService.GetClientAccount(clientId);
            return Ok(clientDto);
        }

    }
}