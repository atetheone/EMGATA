using EMGATA.API.Models;

namespace EMGATA.API.Repositories;

public interface ICarImageRepository : IGenericRepository<CarImage>
{
	Task<IEnumerable<CarImage>> GetImagesByCarAsync(int carId);
	Task<CarImage> GetMainImageByCarAsync(int carId);
	Task SetPrimaryImageAsync(int carId, int imageId);
}