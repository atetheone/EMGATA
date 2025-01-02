using AutoMapper;
using EMGATA.API.Dtos;
using EMGATA.API.Models;
using EMGATA.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMGATA.API.Controllers;

[ApiController]
[Route("api/models")]
public class ModelController : ControllerBase
{
	private readonly IModelService _modelService;
	private readonly IMapper _mapper;

	public ModelController(IModelService modelService, IMapper mapper)
	{
		_modelService = modelService;
		_mapper = mapper;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<ModelDto>>> GetModels()
	{
		var models = await _modelService.GetAllModelsAsync();
		return Ok(_mapper.Map<IEnumerable<ModelDto>>(models));
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<ModelDto>> GetModel(int id)
	{
		var model = await _modelService.GetModelByIdAsync(id);
		if (model == null) return NotFound();
		return Ok(_mapper.Map<ModelDto>(model));
	}
	[HttpGet("brand/{brandId}")]
	public async Task<ActionResult<IEnumerable<ModelDto>>> GetModelsByBrand(int brandId)
	{
		var models = await _modelService.GetModelsByBrandAsync(brandId);
		return Ok(_mapper.Map<IEnumerable<ModelDto>>(models));
	}

	[Authorize(Roles = "Admin")]
	[HttpPost]
	public async Task<ActionResult<ModelDto>> CreateModel(CreateModelDto createModelDto)
	{
		var model = _mapper.Map<Model>(createModelDto);
		var result = await _modelService.CreateModelAsync(model);
		return CreatedAtAction(nameof(GetModel), new { id = result.Id }, _mapper.Map<ModelDto>(result));
	}

	[Authorize(Roles = "Admin")]
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateModel(int id, UpdateModelDto updateModelDto)
	{
	    try
    {
			var existingModel = await _modelService.GetModelByIdAsync(id);
			if (existingModel == null) return NotFound();

			// Check if brand exists before updating
			var model = _mapper.Map(updateModelDto, existingModel);
			await _modelService.UpdateModelAsync(model);
			
			return Ok(_mapper.Map<ModelDto>(model));
    }
    catch (KeyNotFoundException)
    {
			return NotFound("Brand not found");
    }
    catch (Exception ex)
    {
			return StatusCode(500, $"Error updating model: {ex.Message}");
    }
	}

	[Authorize(Roles = "Admin")]
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteModel(int id)
	{
		await _modelService.DeleteModelAsync(id);
		return NoContent();
	}
}