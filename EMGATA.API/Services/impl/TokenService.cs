using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EMGATA.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace EMGATA.API.Services;
public class TokenService : ITokenService
{
	private readonly IConfiguration _configuration;
	private readonly ILogger<TokenService> _logger;

	public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
	{
		_configuration = configuration;
		_logger = logger;
	}

	public string GenerateToken(User user, IList<string> roles)
	{
		try
		{
			var jwtKey = _configuration["Jwt:Key"];
			_logger.LogInformation($"Generating token for user: {user.Email}");

			var claims = new List<Claim>
						{
								new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
								new Claim(ClaimTypes.Name, user.UserName),
								new Claim(ClaimTypes.Email, user.Email),
								new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
						};

			// Add roles to claims
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = creds,
				Issuer = _configuration["Jwt:Issuer"],
				Audience = _configuration["Jwt:Audience"]
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
		catch (Exception ex)
		{
			_logger.LogError($"Error generating token: {ex.Message}");
			throw;
		}
	}
}