using System.ComponentModel.DataAnnotations;

namespace EMGATA.API.Dtos;

public class ModelDto
{
	public int Id { get; set; }
	public BrandDto Brand { get; set; } 
	public string Name { get; set; }
	public string Description { get; set; }
}

public class CreateModelDto
{
	[Required(ErrorMessage = "Brand ID is required")]
	public int BrandId { get; set; }

	[Required(ErrorMessage = "Model name is required")]
	[StringLength(100, ErrorMessage = "Model name cannot be longer than 100 characters")]
	public string Name { get; set; }

	[StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
	public string Description { get; set; }
}

public class UpdateModelDto
{
	[Required(ErrorMessage = "Brand ID is required")]
	public int BrandId { get; set; }

	[Required(ErrorMessage = "Model name is required")]
	[StringLength(100, ErrorMessage = "Model name cannot be longer than 100 characters")]
	public string Name { get; set; }

	[StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
	public string Description { get; set; }
}
