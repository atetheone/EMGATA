using Microsoft.AspNetCore.Http;

namespace EMGATA.API.Services;

public interface IImageStorageService
{
  Task<string> SaveImageAsync(IFormFile imageFile);
  Task DeleteImageAsync(string imageUrl);
}