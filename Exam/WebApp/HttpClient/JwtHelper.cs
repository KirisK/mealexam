using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApp.Extensions;

#pragma warning disable 1591

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