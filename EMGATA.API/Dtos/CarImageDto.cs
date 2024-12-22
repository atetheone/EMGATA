using System.ComponentModel.DataAnnotations;

namespace EMGATA.API.Dtos;

public class CarImageDto
{
	public int Id { get; set; }
	public string ImageUrl { get; set; }
	public bool IsMain { get; set; }
}

public class CreateCarImageDto
{
	[Required(ErrorMessage = "Image URL is required")]
	[StringLength(500, ErrorMessage = "Image URL cannot be longer than 500 characters")]
	[DataType(DataType.ImageUrl)]
	[Url(ErrorMessage = "Please provide a valid URL")]
	public string ImageUrl { get; set; }

	public bool IsMain { get; set; } = false;
}

public class AddCarImageRequest
{
	[Required]
	public IFormFile File { get; set; }
	public bool IsMain { get; set; } = false;
}