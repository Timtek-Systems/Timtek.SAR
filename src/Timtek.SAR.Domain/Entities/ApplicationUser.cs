using Microsoft.AspNetCore.Identity;

namespace Timtek.SAR.Domain.Entities;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public required string DisplayName { get; set; }
    public Guid OrganisationId { get; set; }
    public bool IsActive { get; set; } = true;

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
