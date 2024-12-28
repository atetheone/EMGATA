using EMGATA.API.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EMGATA.Tests.Services;

[TestClass]
public class TokenServiceTests : TestBase
{
  private ITokenService _tokenService;
  private Mock<ILogger<TokenService>> _loggerMock;

  [TestInitialize]
  public void Setup()
  {
    _loggerMock = new Mock<ILogger<TokenService>>();
    _tokenService = new TokenService(_configuration, _loggerMock.Object);
  }

  [TestMethod]
  public void GenerateToken_ShouldReturnValidToken()
  {
    // Arrange
    var user = new EMGATA.API.Models.User
    {
      Id = 1,
      UserName = "testuser",
      Email = "test@example.com"
    };
    var roles = new List<string> { "User" };

    // Act
    var token = _tokenService.GenerateToken(user, roles);

    // Assert
    Assert.IsNotNull(token);
    Assert.IsTrue(token.Length > 0);
  }
}