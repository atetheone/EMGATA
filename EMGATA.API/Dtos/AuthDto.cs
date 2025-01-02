using System.ComponentModel.DataAnnotations;

namespace EMGATA.API.Dtos;
public class LoginDto
{
	[Required(ErrorMessage = "Email is required")]
	[EmailAddress(ErrorMessage = "Invalid email format")]
	public string Email { get; set; }

	[Required(ErrorMessage = "Password is required")]
	public string Password { get; set; }
}

public class RegisterDto
{
	[Required(ErrorMessage = "Username is required")]
	[StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
	public string Username { get; set; }

	[Required(ErrorMessage = "Email is required")]
	[EmailAddress(ErrorMessage = "Invalid email format")]
	public string Email { get; set; }

	[Required(ErrorMessage = "Password is required")]
	[StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
	[DataType(DataType.Password)]
	public string Password { get; set; }
	
}

public class AuthResponseDto
{
	public bool IsSuccess { get; set; }
	public string Message { get; set; }
	public string? Token { get; set; }
	public DateTime? Expiration { get; set; }
}