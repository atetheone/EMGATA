using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Moq;
using System.Text;
using EMGATA.API.Services;

namespace EMGATA.Tests.Services;

[TestClass]
public class ImageStorageServiceTests
{
  private Mock<IWebHostEnvironment> _mockEnvironment;
  private Mock<IHttpContextAccessor> _mockHttpContextAccessor;
  private IImageStorageService _imageStorageService;
  private string _webRootPath;

  [TestInitialize]
  public void Setup()
  {
    // Setup mock environment
    _webRootPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
    Directory.CreateDirectory(_webRootPath);

    _mockEnvironment = new Mock<IWebHostEnvironment>();
    _mockEnvironment.Setup(x => x.WebRootPath).Returns(_webRootPath);

    // Setup mock HTTP context
    var mockHttpContext = new Mock<HttpContext>();
    var mockRequest = new Mock<HttpRequest>();

    mockRequest.Setup(x => x.Scheme).Returns("https");
    mockRequest.Setup(x => x.Host).Returns(new HostString("example.com"));
    mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);

    _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
    _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

    _imageStorageService = new ImageStorageService(_mockEnvironment.Object, _mockHttpContextAccessor.Object);
  }

  [TestCleanup]
  public void Cleanup()
  {
    // Clean up test directory
    if (Directory.Exists(_webRootPath))
    {
      Directory.Delete(_webRootPath, true);
    }
  }

  [TestMethod]
  public async Task SaveImageAsync_ValidJpegImage_ShouldSaveAndReturnUrl()
  {
    // Arrange
    var fileName = "test.jpg";
    var content = "fake image content";
    var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

    var mockFile = new Mock<IFormFile>();
    mockFile.Setup(f => f.FileName).Returns(fileName);
    mockFile.Setup(f => f.Length).Returns(stream.Length);
    mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
        .Callback<Stream, CancellationToken>((stream, token) =>
        {
          // Simulate file copying
        })
        .Returns(Task.CompletedTask);

    // Act
    var result = await _imageStorageService.SaveImageAsync(mockFile.Object);

    // Assert
    Assert.IsNotNull(result);
    Assert.IsTrue(result.StartsWith("https://example.com/Images/"));
    Assert.IsTrue(result.EndsWith(".jpg"));
  }

  [TestMethod]
  [ExpectedException(typeof(ArgumentException))]
  public async Task SaveImageAsync_InvalidFileType_ShouldThrowException()
  {
    // Arrange
    var fileName = "test.txt";
    var mockFile = new Mock<IFormFile>();
    mockFile.Setup(f => f.FileName).Returns(fileName);
    mockFile.Setup(f => f.Length).Returns(100);

    // Act
    await _imageStorageService.SaveImageAsync(mockFile.Object);

    // Assert is handled by ExpectedException
  }

  [TestMethod]
  [ExpectedException(typeof(ArgumentException))]
  public async Task SaveImageAsync_EmptyFile_ShouldThrowException()
  {
    // Arrange
    var mockFile = new Mock<IFormFile>();
    mockFile.Setup(f => f.FileName).Returns("test.jpg");
    mockFile.Setup(f => f.Length).Returns(0);

    // Act
    await _imageStorageService.SaveImageAsync(mockFile.Object);

    // Assert is handled by ExpectedException
  }

  [TestMethod]
  public async Task DeleteImageAsync_ExistingImage_ShouldDeleteFile()
  {
    // Arrange
    var fileName = "test.jpg";
    var filePath = Path.Combine(_webRootPath, "Images", fileName);
    Directory.CreateDirectory(Path.Combine(_webRootPath, "Images"));
    await File.WriteAllTextAsync(filePath, "test content");

    var imageUrl = $"https://example.com/Images/{fileName}";

    // Act
    await _imageStorageService.DeleteImageAsync(imageUrl);

    // Assert
    Assert.IsFalse(File.Exists(filePath));
  }

  [TestMethod]
  public async Task DeleteImageAsync_NonExistingImage_ShouldNotThrowException()
  {
    // Arrange
    var imageUrl = "https://example.com/Images/nonexistent.jpg";

    // Act & Assert
    await _imageStorageService.DeleteImageAsync(imageUrl);
    // Test passes if no exception is thrown
  }

  [TestMethod]
  [ExpectedException(typeof(ArgumentException))]
  public async Task DeleteImageAsync_EmptyUrl_ShouldThrowException()
  {
    // Act
    await _imageStorageService.DeleteImageAsync("");

    // Assert is handled by ExpectedException
  }
}