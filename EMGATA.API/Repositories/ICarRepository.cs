using EMGATA.API.Models;
namespace EMGATA.API.Repositories;

public interface ICarRepository: IGenericRepository<Car>
{
	Task<IEnumerable<Car>> GetAvailableCarsAsync();
	Task<Car> GetCarWithDetailsAsync(int id);
	Task<IEnumerable<Car>> GetCarsByBrandAsync(int brandId);
	Task<IEnumerable<Car>> GetCarsByModelAsync(int modelId);
	Task<IEnumerable<Car>> GetCarsByUserAsync(int userId);
}