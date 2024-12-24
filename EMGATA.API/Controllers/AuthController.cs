using AutoMapper;
using EMGATA.API.Dtos;
using EMGATA.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EMGATA.API.Services;

namespace EMGATA.API.Controllers;
  
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;

    public AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
    {
        // Check if user exists
        var userExists = await _userManager.FindByEmailAsync(registerDto.Email);
        if (userExists != null)
        {
            return BadRequest(new AuthResponseDto 
            { 
                IsSuccess = false,
                Message = "User with this email already exists" 
            });
        }

        var user = new User
        {
            UserName = registerDto.Username,
            Email = registerDto.Email
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        
        if (!result.Succeeded)
        {
            return BadRequest(new AuthResponseDto 
            { 
                IsSuccess = false,
                Message = "Error creating user: " + string.Join(", ", result.Errors.Select(e => e.Description))
            });
        }

        // Add user to default role (User)
        await _userManager.AddToRoleAsync(user, "User");

        // Generate token
        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user, roles);

        return Ok(new AuthResponseDto
        {
            IsSuccess = true,
            Message = "User created successfully",
            Token = token
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return Unauthorized(new AuthResponseDto 
            { 
                IsSuccess = false,
                Message = "Invalid email or password" 
            });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded)
        {
            return Unauthorized(new AuthResponseDto 
            { 
                IsSuccess = false,
                Message = "Invalid email or password" 
            });
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user, roles);

        return Ok(new AuthResponseDto
        {
            IsSuccess = true,
            Message = "Login successful",
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(24)
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok(new { message = "Logged out successfully" });
    }
}