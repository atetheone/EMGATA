using System.ComponentModel.DataAnnotations;
using EMGATA.API.Models.Enums;

namespace EMGATA.API.Models;

public class Car: BaseEntity
{
	[Required]
	public int ModelId { get; set; }

	[Required]
	public int UserId { get; set; }

	[Required]
	public CarStatus Status { get; set; }

	[Required]
	[Range(2010, 2100)]
	public int Year { get; set; }
	
	[Required]
	[StringLength(50)]
	public string Color { get; set; }
	
	[Required]
	[DataType(DataType.Currency)]
	[Range(0, double.MaxValue)]
	public decimal Price { get; set; }
	
	[StringLength(2000)]
	public string Description { get; set; }
	

	// Navigation properties
	public virtual Model Model { get; set; }
	public virtual User User { get; set; }
	public virtual ICollection<CarImage> Images { get; set; }
}
