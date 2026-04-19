using Microsoft.AspNetCore.Identity;
using TA.Utils.Core;
using Timtek.SAR.Application.UserManagement.Dtos;
using Timtek.SAR.Domain.Entities;

namespace Timtek.SAR.Application.UserManagement;

public sealed class UserService(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager) : IUserService
{
    public async Task<UserRegistrationResult> RegisterAsync(RegisterUserRequest request)
    {
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = request.Email,
            Email = request.Email,
            DisplayName = request.DisplayName,
            OrganisationId = request.OrganisationId,
        };

        var result = await userManager.CreateAsync(user, request.Password);

        return result.Succeeded
            ? new UserRegistrationResult(true, user.Id)
            : new UserRegistrationResult(false, Errors: result.Errors.Select(e => e.Description).ToList());
    }

    public async Task<Maybe<UserDto>> GetByIdAsync(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null)
            return Maybe<UserDto>.Empty;

        var roles = await userManager.GetRolesAsync(user);
        return ToDto(user, roles).AsMaybe();
    }

    public async Task<IReadOnlyList<UserDto>> GetByOrganisationAsync(Guid organisationId)
    {
        var users = userManager.Users.Where(u => u.OrganisationId == organisationId).ToList();
        var result = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            result.Add(ToDto(user, roles));
        }

        return result;
    }

    public async Task<bool> AssignRoleAsync(Guid userId, string role)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return false;

        if (!await roleManager.RoleExistsAsync(role))
            return false;

        var result = await userManager.AddToRoleAsync(user, role);
        return result.Succeeded;
    }

    public async Task<bool> RevokeRoleAsync(Guid userId, string role)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return false;

        var result = await userManager.RemoveFromRoleAsync(user, role);
        return result.Succeeded;
    }

    public async Task<bool> DeactivateAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return false;

        user.Deactivate();
        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> ActivateAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return false;

        user.Activate();
        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    static UserDto ToDto(ApplicationUser user, IList<string> roles) =>
        new(user.Id, user.Email!, user.DisplayName, user.OrganisationId, user.IsActive, roles.ToList());
}
