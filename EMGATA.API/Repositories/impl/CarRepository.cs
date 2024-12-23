using EMGATA.API.Data;
using EMGATA.API.Models;
using EMGATA.API.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace EMGATA.API.Repositories;

public class CarRepository : GenericRepository<Car>, ICarRepository
{
	public CarRepository(ApplicationDbContext context) : base(context) { }

	public async Task<IEnumerable<Car>> GetAvailableCarsAsync()
	{
		return await _context.Cars
			.Where(c => c.Status == CarStatus.Available)
			.Include(c => c.Model)
			.ThenInclude(m => m.Brand)
			.Include(c => c.Images.Where(i => i.IsMain))
			.ToListAsync();
	}

	public async Task<Car> GetCarWithDetailsAsync(int carId)
	{
		return await _context.Cars
			.Include(c => c.Model)
			.ThenInclude(m => m.Brand)
			.Include(c => c.Images)
			.Include(c => c.User)
			.FirstOrDefaultAsync(c => c.Id == carId);
	}

	public async Task<IEnumerable<Car>> GetCarsByBrandAsync(int brandId)
	{
		return await _context.Cars
			.Include(c => c.Model)
			.ThenInclude(m => m.Brand)
			.Include(c => c.Images)
			.Where(c => c.Model.BrandId == brandId)
			.ToListAsync();
	}

	public async Task<IEnumerable<Car>> GetCarsByModelAsync(int modelId)
	{
		return await _context.Cars
			.Include(c => c.Model)
			.ThenInclude(m => m.Brand)
			.Include(c => c.Images)
			.Where(c => c.ModelId == modelId)
			.ToListAsync();
	}
	
	public async Task<IEnumerable<Car>> GetCarsByUserAsync(int userId)
	{
		return await _context.Cars
			.Include(c => c.Model)
			.ThenInclude(m => m.Brand)
			.Include(c => c.Images)
			.Where(c => c.UserId == userId)
			.ToListAsync();
	}
}