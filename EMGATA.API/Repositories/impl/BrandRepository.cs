using EMGATA.API.Data;
using EMGATA.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMGATA.API.Repositories;

public class BrandRepository : GenericRepository<Brand>, IBrandRepository
{
	public BrandRepository(ApplicationDbContext context) : base(context) { }

	public async Task<Brand> GetBrandWithModelsAsync(int id)
	{
		return await _context.Brands
			.Include(b => b.Models)
			.FirstOrDefaultAsync(b => b.Id == id);
	}
}