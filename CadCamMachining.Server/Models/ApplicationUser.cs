using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace CadCamMachining.Server.Models;

[CollectionName("Users")]
public class ApplicationUser : MongoIdentityUser<Guid>
{
}

[CollectionName("Roles")]
public class ApplicationRole : MongoIdentityRole<Guid>
{
}