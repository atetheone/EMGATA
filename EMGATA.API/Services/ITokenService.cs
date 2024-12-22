using EMGATA.API.Models;
namespace EMGATA.API.Services;

public interface ITokenService
{
	// Task<string> GenerateToken(User user, string role);
	Task<string> GenerateToken(User user, IList<string> roles);

}
