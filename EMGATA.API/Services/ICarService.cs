using EMGATA.API.Dtos;
using EMGATA.API.Models;
using EMGATA.API.Models.enums;

namespace EMGATA.API.Services
{
	public interface ICarService
	{
		Task<IEnumerable<Car>> GetAllCarsAsync();
		Task<IEnumerable<Car>> GetAvailableCarsAsync();
		Task<Car> GetCarByIdAsync(int id);
		Task<Car> AddCarAsync(Car car);
		Task UpdateCarAsync(Car car);
		Task DeleteCarAsync(int id);
		Task<Car> UpdateCarStatusAsync(int id, CarStatus status);
		Task<IEnumerable<Car>> GetCarsByUserAsync(int userId);
	}
}