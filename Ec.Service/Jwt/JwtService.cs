using Ec.Common.Models.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Ec.Service.Jwt;

public class JwtService
{
    private readonly IConfiguration _configuration;
    private readonly JwtSetting _jwtSetting;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
        _jwtSetting = _configuration.GetSection("JwtSetting").Get<JwtSetting>()!;
    }


    public string GenerateToken(Data.Entities.User user)
    {



        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(ClaimTypes.Role, user.Role)
            };


        var token = new JwtSecurityToken(
            issuer: _jwtSetting.Issuer,
            audience: _jwtSetting.Audience,
            signingCredentials: cred,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}