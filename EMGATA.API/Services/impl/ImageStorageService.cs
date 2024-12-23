using Microsoft.AspNetCore.Http;

namespace EMGATA.API.Services;

public class ImageStorageService : IImageStorageService
{
  private readonly IWebHostEnvironment _environment;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly string _imageDirectory = "Images";

  public ImageStorageService(
      IWebHostEnvironment environment,
      IHttpContextAccessor httpContextAccessor)
  {
    _environment = environment;
    _httpContextAccessor = httpContextAccessor;
  }

  public async Task<string> SaveImageAsync(IFormFile imageFile)
  {
    // Validate file
    if (imageFile == null || imageFile.Length == 0)
    {
      throw new ArgumentException("No file was provided.");
    }

    // Validate file type
    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
    var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

    if (!allowedExtensions.Contains(fileExtension))
    {
      throw new ArgumentException("Invalid file type. Only .jpg, .jpeg and .png are allowed.");
    }

    // Create directory if it doesn't exist
    var uploadsFolder = Path.Combine(_environment.WebRootPath, _imageDirectory);
    if (!Directory.Exists(uploadsFolder))
    {
      Directory.CreateDirectory(uploadsFolder);
    }

    // Generate unique filename
    var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

    // Save file
    using (var fileStream = new FileStream(filePath, FileMode.Create))
    {
      await imageFile.CopyToAsync(fileStream);
    }

    // Generate public URL
    var request = _httpContextAccessor.HttpContext.Request;
    var baseUrl = $"{request.Scheme}://{request.Host}";
    var imageUrl = $"{baseUrl}/{_imageDirectory}/{uniqueFileName}";

    return imageUrl;
  }

  public Task DeleteImageAsync(string imageUrl)
  {
    if (string.IsNullOrEmpty(imageUrl))
    {
      throw new ArgumentException("Image URL is required.");
    }

    // Extract filename from URL
    var uri = new Uri(imageUrl);
    var fileName = Path.GetFileName(uri.LocalPath);
    var filePath = Path.Combine(_environment.WebRootPath, _imageDirectory, fileName);

    // Delete file if exists
    if (File.Exists(filePath))
    {
      File.Delete(filePath);
    }

    return Task.CompletedTask;
  }
}