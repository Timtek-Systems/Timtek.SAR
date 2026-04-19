using Timtek.SAR.Domain.Entities;
using Timtek.SAR.Domain.Enums;

namespace Timtek.SAR.Tests.Domain.Entities;

[Subject("ApplicationUser")]
class When_creating_a_new_application_user
{
    static ApplicationUser _user;

    Because of = () => _user = new ApplicationUser
    {
        DisplayName = "Jane Doe",
        OrganisationId = Guid.NewGuid(),
    };

    It should_be_active_by_default = () => _user.IsActive.ShouldBeTrue();
    It should_have_the_display_name = () => _user.DisplayName.ShouldEqual("Jane Doe");
    It should_have_the_organisation_id = () => _user.OrganisationId.ShouldNotEqual(Guid.Empty);
}

[Subject("ApplicationUser")]
class When_deactivating_a_user
{
    static ApplicationUser _user;

    Establish context = () => _user = new ApplicationUser
    {
        DisplayName = "Jane Doe",
        OrganisationId = Guid.NewGuid(),
    };

    Because of = () => _user.Deactivate();

    It should_be_inactive = () => _user.IsActive.ShouldBeFalse();
}

[Subject("ApplicationUser")]
class When_reactivating_a_user
{
    static ApplicationUser _user;

    Establish context = () =>
    {
        _user = new ApplicationUser
        {
            DisplayName = "Jane Doe",
            OrganisationId = Guid.NewGuid(),
        };
        _user.Deactivate();
    };

    Because of = () => _user.Activate();

    It should_be_active = () => _user.IsActive.ShouldBeTrue();
}

[Subject("UserRole")]
class When_enumerating_user_roles
{
    It should_have_administrator = () => Enum.IsDefined(typeof(UserRole), UserRole.Administrator).ShouldBeTrue();
    It should_have_case_manager = () => Enum.IsDefined(typeof(UserRole), UserRole.CaseManager).ShouldBeTrue();
    It should_have_team_lead = () => Enum.IsDefined(typeof(UserRole), UserRole.TeamLead).ShouldBeTrue();
    It should_have_drone_operator = () => Enum.IsDefined(typeof(UserRole), UserRole.DroneOperator).ShouldBeTrue();
    It should_have_ground_searcher = () => Enum.IsDefined(typeof(UserRole), UserRole.GroundSearcher).ShouldBeTrue();
    It should_have_exactly_five_roles = () => Enum.GetValues<UserRole>().Length.ShouldEqual(5);
}
