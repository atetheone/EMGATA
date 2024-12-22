using EMGATA.API.Models;
using EMGATA.API.Repositories;

namespace EMGATA.API.Services;

public class CarImageService : ICarImageService
{
	private readonly ICarImageRepository _carImageRepository;

	public CarImageService(ICarImageRepository carImageRepository)
	{
		_carImageRepository = carImageRepository;
	}

	public async Task<IEnumerable<CarImage>> GetImagesByCarAsync(int carId)
	{
		return await _carImageRepository.GetImagesByCarAsync(carId);
	}

	public async Task<CarImage> GetMainImageByCarAsync(int carId)
	{
		return await _carImageRepository.GetMainImageByCarAsync(carId);
	}

	public async Task<CarImage> AddImageAsync(int carId, string imageUrl, bool isMain = false)
	{
		var carImage = new CarImage
		{
			CarId = carId,
			ImageUrl = imageUrl,
			IsMain = isMain
		};

		if (isMain)
		{
			var currentMainImage = await GetMainImageByCarAsync(carId);
			if (currentMainImage != null)
			{
				currentMainImage.IsMain = false;
				await _carImageRepository.UpdateAsync(currentMainImage);
			}
		}

		return await _carImageRepository.AddAsync(carImage);
	}

	public async Task DeleteImageAsync(int imageId)
	{
		await _carImageRepository.DeleteAsync(imageId);
	}

	public async Task SetMainImageAsync(int carId, int imageId)
	{
		var currentMainImage = await GetMainImageByCarAsync(carId);
		if (currentMainImage != null)
		{
			currentMainImage.IsMain = false;
			await _carImageRepository.UpdateAsync(currentMainImage);
		}

		var newMainImage = await _carImageRepository.GetByIdAsync(imageId);
		if (newMainImage != null && newMainImage.CarId == carId)
		{
			newMainImage.IsMain = true;
			await _carImageRepository.UpdateAsync(newMainImage);
		}
	}

    public Task<CarImage> GetImageByIdAsync(int imageId)
    {
			return _carImageRepository.GetByIdAsync(imageId);
    }
}