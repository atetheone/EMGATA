using EMGATA.API.Models;

namespace EMGATA.API.Repositories;

public interface IBrandRepository : IGenericRepository<Brand>
{
	Task<Brand> GetBrandWithModelsAsync(int id);
}