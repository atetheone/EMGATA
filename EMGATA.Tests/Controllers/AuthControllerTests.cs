using AutoMapper;
using EMGATA.API.Controllers;
using EMGATA.API.Dtos;
using EMGATA.API.Models;
using EMGATA.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;


[TestClass]
public class AuthControllerTests
{
  private Mock<UserManager<User>> _mockUserManager;
  private Mock<SignInManager<User>> _mockSignInManager;
  private Mock<ITokenService> _mockTokenService;
  private AuthController _controller;

  [TestInitialize]
  public void Setup()
  {
    var userStoreMock = new Mock<IUserStore<User>>();
    _mockUserManager = new Mock<UserManager<User>>(
        userStoreMock.Object, null, null, null, null, null, null, null, null);

    _mockSignInManager = new Mock<SignInManager<User>>(
        _mockUserManager.Object,
        new Mock<IHttpContextAccessor>().Object,
        new Mock<IUserClaimsPrincipalFactory<User>>().Object,
        null, null, null, null);

    _mockTokenService = new Mock<ITokenService>();

    _controller = new AuthController(
        _mockUserManager.Object,
        _mockSignInManager.Object,
        _mockTokenService.Object);
  }

  [TestMethod]
  public async Task Register_WithValidData_ReturnsOkResult()
  {
    // Arrange
    var registerDto = new RegisterDto
    {
      Username = "testuser",
      Email = "test@example.com",
      Password = "Test123!"
    };

    _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
        .ReturnsAsync((User)null);
    _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
        .ReturnsAsync(IdentityResult.Success);
    _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
        .ReturnsAsync(IdentityResult.Success);
    _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
        .ReturnsAsync(new List<string> { "User" });
    _mockTokenService.Setup(x => x.GenerateToken(It.IsAny<User>(), It.IsAny<IList<string>>()))
        .Returns("test-token");

    // Act
    var result = await _controller.Register(registerDto);

    // Assert
    Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    var okResult = result.Result as OkObjectResult;
    Assert.IsNotNull(okResult);

    var response = okResult.Value as AuthResponseDto;
    Assert.IsNotNull(response);
    Assert.IsTrue(response.IsSuccess);
    Assert.IsNotNull(response.Message);
    Assert.AreEqual("User created successfully", response.Message);
  }

  [TestMethod]
  public async Task Login_WithValidCredentials_ReturnsOkResult()
  {
    // Arrange
    var loginDto = new LoginDto
    {
      Email = "test@example.com",
      Password = "Test123!"
    };

    var user = new User { Email = "test@example.com" };

    _mockUserManager.Setup(x => x.FindByEmailAsync(loginDto.Email))
        .ReturnsAsync(user);
    _mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(user, loginDto.Password, false))
        .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
    _mockUserManager.Setup(x => x.GetRolesAsync(user))
        .ReturnsAsync(new List<string> { "User" });
    _mockTokenService.Setup(x => x.GenerateToken(user, It.IsAny<IList<string>>()))
        .Returns("test-token");

    // Act
    var result = await _controller.Login(loginDto);

    // Assert
    Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    var okResult = result.Result as OkObjectResult;
    var response = okResult.Value as AuthResponseDto;
    Assert.IsTrue(response.IsSuccess);
    Assert.IsNotNull(response.Token);
  }
}