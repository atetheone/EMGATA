using System.ComponentModel.DataAnnotations;

namespace EMGATA.API.Dtos;

public class BrandDto
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
}

public class BrandWithModelsDto
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public ICollection<ModelDto> Models { get; set; }
}

public class CreateBrandDto
{
	[Required(ErrorMessage = "Brand name is required")]
	[StringLength(100, ErrorMessage = "Brand name cannot be longer than 100 characters")]
	public string Name { get; set; }

	[StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
	public string Description { get; set; }
}

public class UpdateBrandDto
{
	[Required(ErrorMessage = "Brand name is required")]
	[StringLength(100, ErrorMessage = "Brand name cannot be longer than 100 characters")]
	public string Name { get; set; }

	[StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
	public string Description { get; set; }
}