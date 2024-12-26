using System.ComponentModel.DataAnnotations;
using EMGATA.API.Models.Enums;

namespace EMGATA.API.Dtos;

public class CarDto
{
    public int Id { get; set; }
    public ModelDto Model { get; set; }
    public CarStatus Status { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public ICollection<CarImageDto> Images { get; set; }
}

public class CreateCarDto
{
    [Required(ErrorMessage = "Model ID is required")]
    public int ModelId { get; set; }

    [Required(ErrorMessage = "Year is required")]
    [Range(2010, 2025, ErrorMessage = "Year must be between 2010 and 2025")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Color is required")]
    [StringLength(50, ErrorMessage = "Color cannot be longer than 50 characters")]
    public string Color { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [StringLength(2000, ErrorMessage = "Description cannot be longer than 2000 characters")]
    public string Description { get; set; }
}

public class UpdateCarDto
{
    [Required(ErrorMessage = "Model ID is required")]
    public int ModelId { get; set; }

    [Required(ErrorMessage = "Year is required")]
    [Range(2010, 2100, ErrorMessage = "Year must be between 2010 and 2100")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Color is required")]
    [StringLength(50, ErrorMessage = "Color cannot be longer than 50 characters")]
    public string Color { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [StringLength(2000, ErrorMessage = "Description cannot be longer than 2000 characters")]
    public string Description { get; set; }
}