using System.ComponentModel.DataAnnotations;

namespace EMGATA.API.Models;

public class Brand:	BaseEntity
{
	[Required]
	[StringLength(100)]
	public string Name { get; set; } = string.Empty;

	public string? Description { get; set; }

	public virtual ICollection<Model> Models { get; set; }
}