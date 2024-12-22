using EMGATA.API.Models;

namespace EMGATA.API.Services;

public interface IBrandService
{
	Task<IEnumerable<Brand>> GetAllBrandsAsync();
	Task<Brand> GetBrandByIdAsync(int id);
	Task<Brand> GetBrandWithModelsAsync(int id);
	Task<Brand> CreateBrandAsync(Brand brand);
	Task UpdateBrandAsync(Brand brand);
	Task DeleteBrandAsync(int id);
}