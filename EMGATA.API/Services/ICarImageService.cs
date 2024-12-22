using EMGATA.API.Models;

namespace EMGATA.API.Services;

public interface ICarImageService
{
	Task<IEnumerable<CarImage>> GetImagesByCarAsync(int carId);
	Task<CarImage> GetMainImageByCarAsync(int carId);
	Task<CarImage> AddImageAsync(int carId, string imageUrl, bool isMain = false);
	Task DeleteImageAsync(int imageId);
	Task SetMainImageAsync(int carId, int imageId);
  Task<CarImage> GetImageByIdAsync(int imageId);
}