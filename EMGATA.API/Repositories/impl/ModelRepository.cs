using EMGATA.API.Data;
using EMGATA.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMGATA.API.Repositories;

public class ModelRepository : GenericRepository<Model>, IModelRepository
{
	public ModelRepository(ApplicationDbContext context) : base(context) { }

	public override async Task<IEnumerable<Model>> GetAllAsync()
	{
		return await _context.Models
			.Include(m => m.Brand)
			.ToListAsync();
	}
	
	public async Task<IEnumerable<Model>> GetModelsByBrandAsync(int brandId)
	{
		return await _context.Models
			.Where(m => m.BrandId == brandId)
			.ToListAsync();
	}

	public async Task<Model>? GetModelWithBrandAsync(int id)
	{
		return await _context.Models
			.Include(m => m.Brand)
			.FirstOrDefaultAsync(m => m.Id == id);
	}
}