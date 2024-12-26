using EMGATA.API.Models;
using EMGATA.API.Repositories;

namespace EMGATA.API.Services;

public class BrandService : IBrandService
{
	private readonly IBrandRepository _brandRepository;

	public BrandService(IBrandRepository brandRepository)
	{
		_brandRepository = brandRepository;
	}

	public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
	{
		return await _brandRepository.GetAllAsync();
	}

	public async Task<Brand> GetBrandByIdAsync(int id)
	{
		return await _brandRepository.GetByIdAsync(id);
	}


	public async Task<Brand> CreateBrandAsync(Brand brand)
	{
		return await _brandRepository.AddAsync(brand);
	}

	public async Task UpdateBrandAsync(Brand brand)
	{
		await _brandRepository.UpdateAsync(brand);
	}

	public async Task DeleteBrandAsync(int id)
	{
		await _brandRepository.DeleteAsync(id);
	}
}