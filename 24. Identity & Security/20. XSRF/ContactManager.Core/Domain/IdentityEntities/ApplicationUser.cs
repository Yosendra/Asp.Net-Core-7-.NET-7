using Microsoft.AspNetCore.Identity;

namespace ContactManager.Core.Domain.IdentityEntities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? PersonName { get; set; }     // additional property we define
}
