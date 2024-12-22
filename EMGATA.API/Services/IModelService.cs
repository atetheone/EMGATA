using EMGATA.API.Models;

namespace EMGATA.API.Services;

public interface IModelService
{
	Task<IEnumerable<Model>> GetAllModelsAsync();
	Task<Model> GetModelByIdAsync(int id);
	Task<IEnumerable<Model>> GetModelsByBrandAsync(int brandId);
	Task<Model> CreateModelAsync(Model model);
	Task UpdateModelAsync(Model model);
	Task DeleteModelAsync(int id);
}