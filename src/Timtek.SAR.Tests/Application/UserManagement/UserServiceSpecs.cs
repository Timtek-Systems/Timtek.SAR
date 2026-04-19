using Microsoft.AspNetCore.Identity;
using TA.Utils.Core;
using Timtek.SAR.Application.UserManagement;
using Timtek.SAR.Application.UserManagement.Dtos;
using Timtek.SAR.Domain.Entities;
using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Tests.Application.UserManagement;

class UserServiceContextBuilder
{
    readonly UserManager<ApplicationUser> _userManager;
    readonly RoleManager<ApplicationRole> _roleManager;

    public UserServiceContextBuilder()
    {
        var userStore = A.Fake<IUserStore<ApplicationUser>>();
        _userManager = A.Fake<UserManager<ApplicationUser>>(options =>
            options.WithArgumentsForConstructor([userStore, null, null, null, null, null, null, null, null]));

        var roleStore = A.Fake<IRoleStore<ApplicationRole>>();
        _roleManager = A.Fake<RoleManager<ApplicationRole>>(options =>
            options.WithArgumentsForConstructor([roleStore, null, null, null, null]));
    }

    public UserServiceContextBuilder WithCreateUserSucceeding()
    {
        A.CallTo(() => _userManager.CreateAsync(A<ApplicationUser>._, A<string>._))
            .Returns(Task.FromResult(IdentityResult.Success));
        return this;
    }

    public UserServiceContextBuilder WithCreateUserFailing(params string[] errors)
    {
        var result = IdentityResult.Failed(errors.Select(e => new IdentityError { Description = e }).ToArray());
        A.CallTo(() => _userManager.CreateAsync(A<ApplicationUser>._, A<string>._))
            .Returns(Task.FromResult(result));
        return this;
    }

    public UserServiceContextBuilder WithExistingUser(ApplicationUser user, params string[] roles)
    {
        A.CallTo(() => _userManager.FindByIdAsync(user.Id.ToString()))
            .Returns(Task.FromResult<ApplicationUser?>(user));
        A.CallTo(() => _userManager.GetRolesAsync(user))
            .Returns(Task.FromResult<IList<string>>(roles.ToList()));
        return this;
    }

    public UserServiceContextBuilder WithNoUser(Guid id)
    {
        A.CallTo(() => _userManager.FindByIdAsync(id.ToString()))
            .Returns(Task.FromResult<ApplicationUser?>(null));
        return this;
    }

    public UserServiceContextBuilder WithRoleExists(string role)
    {
        A.CallTo(() => _roleManager.RoleExistsAsync(role))
            .Returns(Task.FromResult(true));
        A.CallTo(() => _userManager.AddToRoleAsync(A<ApplicationUser>._, role))
            .Returns(Task.FromResult(IdentityResult.Success));
        A.CallTo(() => _userManager.RemoveFromRoleAsync(A<ApplicationUser>._, role))
            .Returns(Task.FromResult(IdentityResult.Success));
        return this;
    }

    public UserServiceContextBuilder WithUpdateSucceeding()
    {
        A.CallTo(() => _userManager.UpdateAsync(A<ApplicationUser>._))
            .Returns(Task.FromResult(IdentityResult.Success));
        return this;
    }

    public IUserService Build() => new UserService(_userManager, _roleManager);

    public UserManager<ApplicationUser> UserManager => _userManager;
    public RoleManager<ApplicationRole> RoleManager => _roleManager;
}

// --- Registration ---

[Subject("User Service")]
class When_registering_a_new_user
{
    static UserServiceContextBuilder _context;
    static IUserService _service;
    static UserRegistrationResult _result;
    static Guid _organisationId;

    Establish context = () =>
    {
        _organisationId = Guid.NewGuid();
        _context = new UserServiceContextBuilder()
            .WithCreateUserSucceeding();
        _service = _context.Build();
    };

    Because of = () => _result = _service.RegisterAsync(new RegisterUserRequest(
        "john@example.com", "John Doe", "P@ssw0rd!", _organisationId)).GetAwaiter().GetResult();

    It should_succeed = () => _result.Succeeded.ShouldBeTrue();
    It should_return_a_user_id = () => _result.UserId.ShouldNotBeNull();
    It should_call_create_on_user_manager = () =>
        A.CallTo(() => _context.UserManager.CreateAsync(
            A<ApplicationUser>.That.Matches(u =>
                u.Email == "john@example.com" &&
                u.DisplayName == "John Doe" &&
                u.OrganisationId == _organisationId),
            "P@ssw0rd!"))
            .MustHaveHappenedOnceExactly();
}

[Subject("User Service")]
class When_registering_a_user_with_invalid_password
{
    static UserServiceContextBuilder _context;
    static IUserService _service;
    static UserRegistrationResult _result;

    Establish context = () =>
    {
        _context = new UserServiceContextBuilder()
            .WithCreateUserFailing("Password too short");
        _service = _context.Build();
    };

    Because of = () => _result = _service.RegisterAsync(new RegisterUserRequest(
        "john@example.com", "John", "short", Guid.NewGuid())).GetAwaiter().GetResult();

    It should_not_succeed = () => _result.Succeeded.ShouldBeFalse();
    It should_return_the_error = () => _result.Errors.ShouldContain("Password too short");
}

// --- Get by ID ---

[Subject("User Service")]
class When_getting_an_existing_user_by_id
{
    static UserServiceContextBuilder _context;
    static IUserService _service;
    static ApplicationUser _existing;
    static Maybe<UserDto> _result;
    static Guid _organisationId;

    Establish context = () =>
    {
        _organisationId = Guid.NewGuid();
        _existing = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = "john@example.com",
            UserName = "john@example.com",
            DisplayName = "John Doe",
            OrganisationId = _organisationId,
        };
        _context = new UserServiceContextBuilder()
            .WithExistingUser(_existing, "Administrator");
        _service = _context.Build();
    };

    Because of = () => _result = _service.GetByIdAsync(_existing.Id).GetAwaiter().GetResult();

    It should_return_the_user = () => _result.Any().ShouldBeTrue();
    It should_have_the_correct_email = () => _result.Single().Email.ShouldEqual("john@example.com");
    It should_have_the_correct_display_name = () => _result.Single().DisplayName.ShouldEqual("John Doe");
    It should_include_roles = () => _result.Single().Roles.ShouldContain("Administrator");
}

[Subject("User Service")]
class When_getting_a_nonexistent_user_by_id
{
    static UserServiceContextBuilder _context;
    static IUserService _service;
    static Guid _missingId;
    static Maybe<UserDto> _result;

    Establish context = () =>
    {
        _missingId = Guid.NewGuid();
        _context = new UserServiceContextBuilder().WithNoUser(_missingId);
        _service = _context.Build();
    };

    Because of = () => _result = _service.GetByIdAsync(_missingId).GetAwaiter().GetResult();

    It should_return_empty = () => _result.Any().ShouldBeFalse();
}

// --- Role assignment ---

[Subject("User Service")]
class When_assigning_a_role_to_a_user
{
    static UserServiceContextBuilder _context;
    static IUserService _service;
    static ApplicationUser _existing;
    static bool _result;

    Establish context = () =>
    {
        _existing = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = "john@example.com",
            UserName = "john@example.com",
            DisplayName = "John Doe",
            OrganisationId = Guid.NewGuid(),
        };
        _context = new UserServiceContextBuilder()
            .WithExistingUser(_existing)
            .WithRoleExists("CaseManager");
        _service = _context.Build();
    };

    Because of = () => _result = _service.AssignRoleAsync(_existing.Id, "CaseManager").GetAwaiter().GetResult();

    It should_succeed = () => _result.ShouldBeTrue();
    It should_add_the_role = () =>
        A.CallTo(() => _context.UserManager.AddToRoleAsync(_existing, "CaseManager"))
            .MustHaveHappenedOnceExactly();
}

[Subject("User Service")]
class When_assigning_a_role_to_a_nonexistent_user
{
    static UserServiceContextBuilder _context;
    static IUserService _service;
    static Guid _missingId;
    static bool _result;

    Establish context = () =>
    {
        _missingId = Guid.NewGuid();
        _context = new UserServiceContextBuilder().WithNoUser(_missingId);
        _service = _context.Build();
    };

    Because of = () => _result = _service.AssignRoleAsync(_missingId, "CaseManager").GetAwaiter().GetResult();

    It should_return_false = () => _result.ShouldBeFalse();
}

// --- Role revocation ---

[Subject("User Service")]
class When_revoking_a_role_from_a_user
{
    static UserServiceContextBuilder _context;
    static IUserService _service;
    static ApplicationUser _existing;
    static bool _result;

    Establish context = () =>
    {
        _existing = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = "john@example.com",
            UserName = "john@example.com",
            DisplayName = "John Doe",
            OrganisationId = Guid.NewGuid(),
        };
        _context = new UserServiceContextBuilder()
            .WithExistingUser(_existing, "CaseManager")
            .WithRoleExists("CaseManager");
        _service = _context.Build();
    };

    Because of = () => _result = _service.RevokeRoleAsync(_existing.Id, "CaseManager").GetAwaiter().GetResult();

    It should_succeed = () => _result.ShouldBeTrue();
    It should_remove_the_role = () =>
        A.CallTo(() => _context.UserManager.RemoveFromRoleAsync(_existing, "CaseManager"))
            .MustHaveHappenedOnceExactly();
}

// --- Deactivation ---

[Subject("User Service")]
class When_deactivating_a_user
{
    static UserServiceContextBuilder _context;
    static IUserService _service;
    static ApplicationUser _existing;
    static bool _result;

    Establish context = () =>
    {
        _existing = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = "john@example.com",
            UserName = "john@example.com",
            DisplayName = "John Doe",
            OrganisationId = Guid.NewGuid(),
        };
        _context = new UserServiceContextBuilder()
            .WithExistingUser(_existing)
            .WithUpdateSucceeding();
        _service = _context.Build();
    };

    Because of = () => _result = _service.DeactivateAsync(_existing.Id).GetAwaiter().GetResult();

    It should_succeed = () => _result.ShouldBeTrue();
    It should_set_user_inactive = () => _existing.IsActive.ShouldBeFalse();
    It should_update_the_user = () =>
        A.CallTo(() => _context.UserManager.UpdateAsync(_existing)).MustHaveHappenedOnceExactly();
}

[Subject("User Service")]
class When_deactivating_a_nonexistent_user
{
    static UserServiceContextBuilder _context;
    static IUserService _service;
    static Guid _missingId;
    static bool _result;

    Establish context = () =>
    {
        _missingId = Guid.NewGuid();
        _context = new UserServiceContextBuilder().WithNoUser(_missingId);
        _service = _context.Build();
    };

    Because of = () => _result = _service.DeactivateAsync(_missingId).GetAwaiter().GetResult();

    It should_return_false = () => _result.ShouldBeFalse();
}

// --- Activation ---

[Subject("User Service")]
class When_activating_a_deactivated_user
{
    static UserServiceContextBuilder _context;
    static IUserService _service;
    static ApplicationUser _existing;
    static bool _result;

    Establish context = () =>
    {
        _existing = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = "john@example.com",
            UserName = "john@example.com",
            DisplayName = "John Doe",
            OrganisationId = Guid.NewGuid(),
        };
        _existing.Deactivate();

        _context = new UserServiceContextBuilder()
            .WithExistingUser(_existing)
            .WithUpdateSucceeding();
        _service = _context.Build();
    };

    Because of = () => _result = _service.ActivateAsync(_existing.Id).GetAwaiter().GetResult();

    It should_succeed = () => _result.ShouldBeTrue();
    It should_set_user_active = () => _existing.IsActive.ShouldBeTrue();
    It should_update_the_user = () =>
        A.CallTo(() => _context.UserManager.UpdateAsync(_existing)).MustHaveHappenedOnceExactly();
}
