using Microsoft.AspNetCore.Identity;

namespace CadCamMachining.Server.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ICollection<Order>? Orders { get; set; } = new List<Order>();
    }
}
