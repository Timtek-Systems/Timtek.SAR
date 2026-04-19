using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Timtek.Patterns.DataAccess;
using Timtek.SAR.Domain.Entities;
using Timtek.SAR.Infrastructure.Persistence;

namespace Timtek.SAR.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddSarInfrastructure(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> configureDatabase)
    {
        services.AddDbContext<SarDbContext>(configureDatabase);
        services.AddScoped<SarUnitOfWork>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<SarUnitOfWork>());
        services.AddScoped<IRepository<Organisation, Guid>>(sp =>
            sp.GetRequiredService<SarUnitOfWork>().Organisations);

        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<SarDbContext>();

        return services;
    }
}
