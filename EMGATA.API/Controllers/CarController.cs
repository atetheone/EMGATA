using System.Security.Claims;
using AutoMapper;
using EMGATA.API.Dtos;
using EMGATA.API.Models;
using EMGATA.API.Models.Enums;
using EMGATA.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMGATA.API.Controllers;

[ApiController]
[Route("api/cars")]
public class CarController : ControllerBase
{
	private readonly ICarService _carService;
	private readonly ICarImageService _carImageService;
	private readonly IImageStorageService _imageStorageService;
	private readonly IMapper _mapper;

	public CarController(
			ICarService carService,
			ICarImageService carImageService,
			IImageStorageService imageStorageService,
			IMapper mapper)
	{
		_carService = carService;
		_carImageService = carImageService;
		_imageStorageService = imageStorageService;
		_mapper = mapper;
	}
	[HttpGet]
	public async Task<ActionResult<IEnumerable<CarDto>>> GetCars()
	{
		var cars = await _carService.GetAllCarsAsync();
		return Ok(_mapper.Map<IEnumerable<CarDto>>(cars));
	}

	[HttpGet("available")]
	public async Task<ActionResult<IEnumerable<CarDto>>> GetAvailableCars()
	{
		var cars = await _carService.GetAvailableCarsAsync();
		return Ok(_mapper.Map<IEnumerable<CarDto>>(cars));
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<CarDto>> GetCar(int id)
	{
		var car = await _carService.GetCarByIdAsync(id);
		return Ok(_mapper.Map<CarDto>(car));
	}

	[Authorize(Roles = "Admin")]
	[HttpPost]
	public async Task<ActionResult<CarDto>> CreateCar(CreateCarDto createCarDto)
	{
		var car = _mapper.Map<Car>(createCarDto);
		car.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
		car.Status = CarStatus.Available;

		var result = await _carService.AddCarAsync(car);
		return CreatedAtAction(nameof(GetCar), new { id = result.Id }, _mapper.Map<CarDto>(result));
	}

	[Authorize(Roles = "Admin")]
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateCar(int id, UpdateCarDto updateCarDto)
	{
		var car = await _carService.GetCarByIdAsync(id);
		_mapper.Map(updateCarDto, car);
		await _carService.UpdateCarAsync(car);
		return NoContent();
	}

	[Authorize(Roles = "Admin")]
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteCar(int id)
	{
		await _carService.DeleteCarAsync(id);
		return NoContent();
	}

	[Authorize(Roles = "Admin")]
	[HttpPatch("{id}/status")]
	public async Task<ActionResult<CarDto>> UpdateCarStatus(int id, [FromBody] CarStatus status)
	{
		var car = await _carService.UpdateCarStatusAsync(id, status);
		return Ok(_mapper.Map<CarDto>(car));
	}

	// Car Images endpoints
	[HttpGet("{carId}/images")]
	public async Task<ActionResult<IEnumerable<CarImageDto>>> GetCarImages(int carId)
	{
		var images = await _carImageService.GetImagesByCarAsync(carId);
		return Ok(_mapper.Map<IEnumerable<CarImageDto>>(images));
	}
	
	[Authorize(Roles = "Admin")]
	[HttpPost("{carId}/images")]
	[Consumes("multipart/form-data")]
	public async Task<ActionResult<CarImageDto>> AddCarImage(int carId, [FromForm] AddCarImageRequest request)
	{
		try
		{
			// Save image and get URL
			var imageUrl = await _imageStorageService.SaveImageAsync(request.File);

			// Create image in database
			var result = await _carImageService.AddImageAsync(carId, imageUrl, request.IsMain);

			return Ok(_mapper.Map<CarImageDto>(result));

		}
		catch (ArgumentException ex)
		{
			return BadRequest(ex.Message);
		}
		catch (Exception)
		{
			return StatusCode(500, "Error uploading image");
		}

	}

	[Authorize(Roles = "Admin")]
	[HttpDelete("{carId}/images/{imageId}")]
	public async Task<IActionResult> DeleteCarImage(int carId, int imageId)
	{
		try
		{
			// Get image URL before deleting
			var image = await _carImageService.GetImageByIdAsync(imageId);

			// Delete image file
			await _imageStorageService.DeleteImageAsync(image.ImageUrl);

			// Delete from database
			await _carImageService.DeleteImageAsync(imageId);

			return NoContent();
		}
		catch (KeyNotFoundException)
		{
			return NotFound();
		}
		catch (Exception)
		{
			return StatusCode(500, "Error deleting image");
		}
	}

	[Authorize(Roles = "Admin")]
	[HttpPut("{carId}/images/{imageId}/set-main")]
	public async Task<IActionResult> SetMainImage(int carId, int imageId)
	{
		await _carImageService.SetMainImageAsync(carId, imageId);
		return NoContent();
	}
}