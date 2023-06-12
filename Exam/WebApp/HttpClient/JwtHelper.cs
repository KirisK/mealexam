using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.App.Identity;
using Microsoft.AspNetCore.Identity;
using WebApp.Extensions;

namespace WebApp.HttpClient;

public class JwtHelper
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly IConfiguration _configuration;

    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetJwt(ClaimsPrincipal claimsPrincipal)
    {
        var jwtClaim = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == "JwtToken");
        return jwtClaim != null 
            ? jwtClaim.Value 
            : _tokenHandler.GenerateJwt(claimsPrincipal.Claims, _configuration);
    }
}