using TA.Utils.Core;
using Timtek.SAR.Application.UserManagement.Dtos;
using Timtek.SAR.Domain.Entities;

namespace Timtek.SAR.Application.UserManagement;

public interface IUserService
{
    Task<UserRegistrationResult> RegisterAsync(RegisterUserRequest request);
    Task<Maybe<UserDto>> GetByIdAsync(Guid id);
    Task<IReadOnlyList<UserDto>> GetByOrganisationAsync(Guid organisationId);
    Task<bool> AssignRoleAsync(Guid userId, string role);
    Task<bool> RevokeRoleAsync(Guid userId, string role);
    Task<bool> DeactivateAsync(Guid userId);
    Task<bool> ActivateAsync(Guid userId);
}
