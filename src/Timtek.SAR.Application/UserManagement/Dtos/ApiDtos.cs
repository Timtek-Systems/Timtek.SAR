namespace Timtek.SAR.Application.UserManagement.Dtos;

public sealed record RegisterRequest(string Email, string DisplayName, string Passphrase, Guid OrganisationId);
public sealed record LoginRequest(string Email, string Passphrase);

public sealed record RegisterResponse(bool Succeeded, Guid? UserId = null, IReadOnlyList<string>? Errors = null);
public sealed record UserResponse(Guid Id, string Email, string DisplayName, Guid OrganisationId, bool IsActive, IReadOnlyList<string> Roles);
public sealed record AssignRoleRequest(string Role);
