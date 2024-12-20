using System.ComponentModel.DataAnnotations;

namespace EMGATA.API.Models;

public class CarImage: BaseEntity
{
	[Required]
	public int CarId { get; set; }
	
	public virtual Car Car { get; set; }
    
	[Required]
	[Url]
	public string ImageUrl { get; set; }
    
	public bool IsMain { get; set; }
}