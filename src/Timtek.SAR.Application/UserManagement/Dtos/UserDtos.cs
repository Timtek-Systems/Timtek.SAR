namespace Timtek.SAR.Application.UserManagement.Dtos;

public sealed record RegisterUserRequest(
    string Email,
    string DisplayName,
    string Password,
    Guid OrganisationId);

public sealed record UserRegistrationResult(
    bool Succeeded,
    Guid? UserId = null,
    IReadOnlyList<string>? Errors = null);

public sealed record UserDto(
    Guid Id,
    string Email,
    string DisplayName,
    Guid OrganisationId,
    bool IsActive,
    IReadOnlyList<string> Roles);
