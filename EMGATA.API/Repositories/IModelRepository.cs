using EMGATA.API.Models;

namespace EMGATA.API.Repositories;

public interface IModelRepository : IGenericRepository<Model>
{
	Task<IEnumerable<Model>> GetModelsByBrandAsync(int brandId);
	Task<Model> GetModelWithBrandAsync(int id);
}