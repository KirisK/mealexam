/*using System.IdentityModel.Tokens.Jwt;
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
        /// Create UserContext object from ClaimsPrincipal
        /// </summary>
        /// <param name="user">ClaimsPrincipal</param>
        /// <returns>UserContext</returns>
        public static UserContext AsAppUserContext(this ClaimsPrincipal user)
        {
            return new()
            {
                UserId = user.GetUserId(),
                UserRole = user.GetUserRole()
            };
        }

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
        /// Get user role from ClaimsPrincipal
        /// </summary>
        /// <param name="user">ClaimsPrincipal</param>
        /// <returns>UserRole</returns>
        public static UserRole GetUserRole(this ClaimsPrincipal user)
        {
            var bestRole = UserRole.Unauthorized;
            foreach (var role in (UserRole[])Enum.GetValues(typeof(UserRole)))
            {
                if (user.IsInRole(role.ToString()))
                {
                    bestRole = role;
                }
            }

            return bestRole;
        }

        /// <summary>
        /// Get custom claim value from ClaimsPrincipal
        /// </summary>
        /// <param name="user">ClaimsPrincipal</param>
        /// <param name="claim">Claim name</param>
        /// <param name="toValue">string to value type mapper function</param>
        /// <param name="defaultValue">default value</param>
        /// <returns>Typed claim value</returns>
        public static TValue? GetClaimValue<TValue>(
            this ClaimsPrincipal user,
            string claim,
            Func<string, TValue> toValue,
            TValue? defaultValue = default
        )
        {
            var val = user.Claims.FirstOrDefault(c => c.Type == claim)?.Value;
            return val != null ? toValue(val) : defaultValue;
        }

        /// <summary>
        /// Get custom claim value from ClaimsPrincipal
        /// </summary>
        /// <param name="user">ClaimsPrincipal</param>
        /// <param name="claim">Claim name</param>
        /// <returns>Typed claim value</returns>
        public static bool HasClaimValue(
            this ClaimsPrincipal user,
            string claim
        )
        {
            return user.Claims.FirstOrDefault(c => c.Type == claim)?.Value != null;
        }

        /// <summary>
        /// Get custom claim values list from ClaimsPrincipal
        /// </summary>
        /// <param name="user">ClaimsPrincipal</param>
        /// <param name="claim">Claim name</param>
        /// <param name="toValue">string to value type mapper function</param>
        /// <returns>Typed claim values</returns>
        public static List<TValue> GetClaimValues<TValue>(
            this ClaimsPrincipal user,
            string claim,
            Func<string, TValue> toValue
        )
        {
            return user.Claims
                .Where(c => c.Type == claim)
                .Select(c => c.Value)
                .Select(toValue)
                .ToList();
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
                claims,
                expires: DateTime.Now.AddDays(config.GetValue<int>("JWT:ExpireDays")),
                signingCredentials: signingCredentials
            );
            return handler.WriteToken(token);
        }

        /// <summary>
        /// If user's role is admin
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsAdmin(this ClaimsPrincipal user) => user.IsInRole(nameof(UserRole.Admin));

        /// <summary>
        /// If user's role is regular user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsRegularUser(this ClaimsPrincipal user) =>
            user.IsInRole(nameof(UserRole.User));

    }*/