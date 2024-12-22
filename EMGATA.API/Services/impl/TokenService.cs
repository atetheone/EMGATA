using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EMGATA.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace EMGATA.API.Services
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _configuration;

		public TokenService(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		
		public Task<string> GenerateToken(User user, IList<string> roles)
		{
			var jwtKey = _configuration["Jwt:Key"];
			if (string.IsNullOrEmpty(jwtKey))
			{
				throw new InvalidOperationException("JWT Key is not configured");
			}
			
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email)
			};

			// Add roles to claims
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddHours(24),
				signingCredentials: creds
			);
			
			return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
		}
		
		public Task<string> GenerateToken2(User user, string role)
		{
			var jwtKey = _configuration["Jwt:Key"];
			if (string.IsNullOrEmpty(jwtKey))
			{
				throw new ArgumentNullException(nameof(jwtKey), "JWT key cannot be null or empty.");
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(ClaimTypes.Role, role)
			};
			

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(30),
				signingCredentials: creds);

			return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
		}
	}
}