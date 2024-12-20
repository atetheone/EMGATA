using Microsoft.AspNetCore.Identity;

namespace EMGATA.API.Models;

public class User: IdentityUser<int>
{
	public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}