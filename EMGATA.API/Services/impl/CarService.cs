using EMGATA.API.Dtos;
using EMGATA.API.Models;
using EMGATA.API.Repositories;
using AutoMapper;
using EMGATA.API.Models.Enums;

namespace EMGATA.API.Services;

public class CarService : ICarService
{
	private readonly ICarRepository _carRepository;
	private readonly IModelRepository _modelRepository;

	public CarService(ICarRepository carRepository, IModelRepository modelRepository)
	{
		_carRepository = carRepository;
		_modelRepository = modelRepository;
	}

	public async Task<IEnumerable<Car>> GetAllCarsAsync()
	{
		return await _carRepository.GetAllAsync();
	}

	public async Task<IEnumerable<Car>> GetAvailableCarsAsync()
	{
		return await _carRepository.GetAvailableCarsAsync();
	}

	public async Task<Car> GetCarByIdAsync(int id)
	{
		return await _carRepository.GetCarWithDetailsAsync(id);
	}

	public async Task<Car> AddCarAsync(Car car)
	{
		// Validate model exists
		await _modelRepository.GetByIdAsync(car.ModelId);

		// Validate year
		if (car.Year < 2010)
			throw new ArgumentException("Car year must be 2010 or later");

		return await _carRepository.AddAsync(car);
	}

	public async Task UpdateCarAsync(Car car)
	{
		// Validate exists
		await _carRepository.GetByIdAsync(car.Id);
        
		// Validate model exists
		await _modelRepository.GetByIdAsync(car.ModelId);

		// Validate year
		if (car.Year < 2010)
			throw new ArgumentException("Car year must be 2010 or later");

		await _carRepository.UpdateAsync(car);
	}

	public async Task DeleteCarAsync(int id)
	{
		await _carRepository.DeleteAsync(id);
	}

	public async Task<Car> UpdateCarStatusAsync(int id, CarStatus status)
	{
		var car = await _carRepository.GetByIdAsync(id);
		car.Status = status;
		await _carRepository.UpdateAsync(car);
		return car;
	}

	public async Task<IEnumerable<Car>> GetCarsByUserAsync(int userId)
	{
		return await _carRepository.GetCarsByUserAsync(userId);
	}
}