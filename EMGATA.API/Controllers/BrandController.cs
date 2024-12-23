using AutoMapper;
using EMGATA.API.Dtos;
using EMGATA.API.Models;
using EMGATA.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMGATA.API.Controllers;

[ApiController]
[Route("api/brands")]
public class BrandController : ControllerBase
{
	private readonly IBrandService _brandService;
	private readonly IMapper _mapper;

	public BrandController(IBrandService brandService, IMapper mapper)
	{
		_brandService = brandService;
		_mapper = mapper;
	}
	
	[HttpGet]
	public async Task<ActionResult<IEnumerable<BrandDto>>> GetBrands()
	{
		var brands = await _brandService.GetAllBrandsAsync();
		return Ok(_mapper.Map<IEnumerable<BrandDto>>(brands));
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<BrandDto>> GetBrand(int id)
	{
		var brand = await _brandService.GetBrandByIdAsync(id);
		return Ok(_mapper.Map<BrandDto>(brand));
	}

	[HttpGet("{id}/models")]
	public async Task<ActionResult<BrandDto>> GetBrandWithModels(int id)
	{
		var brand = await _brandService.GetBrandWithModelsAsync(id);
		return Ok(_mapper.Map<BrandDto>(brand));
	}

	[Authorize(Roles = "Admin")]
	[HttpPost]
	public async Task<ActionResult<BrandDto>> CreateBrand(CreateBrandDto createBrandDto)
	{
		var brand = _mapper.Map<Brand>(createBrandDto);
		var result = await _brandService.CreateBrandAsync(brand);
		return CreatedAtAction(nameof(GetBrand), new { id = result.Id }, _mapper.Map<BrandDto>(result));
	}

	[Authorize(Roles = "Admin")]
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateBrand(int id, UpdateBrandDto updateBrandDto)
	{
		var brand = await _brandService.GetBrandByIdAsync(id);
		_mapper.Map(updateBrandDto, brand);
		await _brandService.UpdateBrandAsync(brand);
		return NoContent();
	}

	[Authorize(Roles = "Admin")]
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteBrand(int id)
	{
		await _brandService.DeleteBrandAsync(id);
		return NoContent();
	}

}