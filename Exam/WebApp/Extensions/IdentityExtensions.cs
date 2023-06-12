using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.App;
using Microsoft.IdentityModel.Tokens;

namespace WebApp.Extensions;

/// <summary>
/// Identity Extensions
/// </summary>
public static class IdentityExtensions
{
    /// <summary>
    /// Get user Id from ClaimsPrincipal
    /// </summary>
    /// <param name="user">ClaimsPrincipal</param>
    /// <returns>Id</returns>
    public static Guid? GetUserId(this ClaimsPrincipal user)
    {
        var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
        {
            return Guid.Parse(userId);
        }

        return null;
    }
    
    /// <summary>
    /// Generate JWT token
    /// </summary>
    /// <param name="handler">JWT handler</param>
    /// <param name="claims">claims to be encoded</param>
    /// <param name="config">JWT configuration</param>
    /// <returns>JWT token</returns>
    public static string GenerateJwt(
        this JwtSecurityTokenHandler handler,
        IEnumerable<Claim> claims,
        IConfiguration config
    )
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]!));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            config["JWT:Issuer"],
            config["JWT:Audience"],
            claims.Where(cl => cl.Type != "JwtToken"),
            expires: DateTime.Now.AddDays(config.GetValue<int>("JWT:ExpireDays")),
            signingCredentials: signingCredentials
        );
        return handler.WriteToken(token);
    }
}