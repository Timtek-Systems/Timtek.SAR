using Microsoft.AspNetCore.Identity;

namespace Timtek.SAR.Domain.Entities;

public sealed class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole() { }
    public ApplicationRole(string roleName) : base(roleName) { }
}
