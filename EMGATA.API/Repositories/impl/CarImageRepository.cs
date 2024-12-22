using EMGATA.API.Data;
using EMGATA.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMGATA.API.Repositories;

public class CarImageRepository : GenericRepository<CarImage>, ICarImageRepository
{
	public CarImageRepository(ApplicationDbContext context) : base(context) { }

	public async Task<IEnumerable<CarImage>> GetImagesByCarAsync(int carId)
	{
		return await _context.CarImages
			.Where(ci => ci.CarId == carId)
			.ToListAsync();
	}

	public async Task<CarImage> GetMainImageByCarAsync(int carId)
	{
		return await _context.CarImages
			.FirstOrDefaultAsync(ci => ci.CarId == carId && ci.IsMain);
	}
	
	public async Task SetPrimaryImageAsync(int carId, int imageId)
	{
		// Reset all images for this car to non-primary
		var carImages = await _context.CarImages
			.Where(i => i.CarId == carId)
			.ToListAsync();

		foreach (var image in carImages)
		{
			image.IsMain = image.Id == imageId;
		}
	}
}