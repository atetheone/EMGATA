using EMGATA.API.Data;
using EMGATA.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMGATA.API.Repositories;

public class BrandRepository : GenericRepository<Brand>, IBrandRepository
{
	public BrandRepository(ApplicationDbContext context) : base(context) { }

	public override async Task<IEnumerable<Brand>> GetAllAsync()
	{
		return await _context.Brands
			.ToListAsync();
	}

}