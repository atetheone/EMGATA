using EMGATA.API.Models;
using EMGATA.API.Repositories;

namespace EMGATA.API.Services;

public class ModelService : IModelService
{
	private readonly IModelRepository _modelRepository;
	private readonly IBrandRepository _brandRepository;

	public ModelService(IModelRepository modelRepository, IBrandRepository brandRepository)
	{
		_modelRepository = modelRepository;
		_brandRepository = brandRepository;
	}

	public async Task<IEnumerable<Model>> GetAllModelsAsync()
	{
		return await _modelRepository.GetAllAsync();
	}

	public async Task<Model> GetModelByIdAsync(int id)
	{
		return await _modelRepository.GetByIdAsync(id);
	}

	public async Task<IEnumerable<Model>> GetModelsByBrandAsync(int brandId)
	{
		// Validate brand exists
		await _brandRepository.GetByIdAsync(brandId);
		return await _modelRepository.GetModelsByBrandAsync(brandId);
	}

	public async Task<Model> CreateModelAsync(Model model)
	{
		// Validate brand exists
		await _brandRepository.GetByIdAsync(model.BrandId);
		return await _modelRepository.AddAsync(model);
	}

	public async Task UpdateModelAsync(Model model)
	{
		// Validate model exists
		await _modelRepository.GetByIdAsync(model.Id);
        
		// Validate brand exists
		await _brandRepository.GetByIdAsync(model.BrandId);
        
		await _modelRepository.UpdateAsync(model);
	}

	public async Task DeleteModelAsync(int id)
	{
		await _modelRepository.DeleteAsync(id);
	}
}