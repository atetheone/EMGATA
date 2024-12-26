using System.ComponentModel.DataAnnotations;

namespace EMGATA.API.Models;

public class Model : BaseEntity
{
	[Required]
	public int BrandId { get; set; }
	[Required]
	public string Name { get; set; }
	public string Description { get; set; }

	// Navigation properties
	public virtual Brand Brand { get; set; }
	public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
