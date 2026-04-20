using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Timtek.Patterns.DataAccess;
using Timtek.SAR.Application.Geospatial;
using Timtek.SAR.Domain.Entities;
using Timtek.SAR.Infrastructure.Geospatial;
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
        services.AddScoped<IRepository<Case, Guid>>(sp =>
            sp.GetRequiredService<SarUnitOfWork>().Cases);
        services.AddScoped<IRepository<CaseActivityLog, Guid>>(sp =>
            sp.GetRequiredService<SarUnitOfWork>().CaseActivityLogs);
        services.AddScoped<IRepository<SearchArea, Guid>>(sp =>
            sp.GetRequiredService<SarUnitOfWork>().SearchAreas);
        services.AddScoped<IRepository<Sector, Guid>>(sp =>
            sp.GetRequiredService<SarUnitOfWork>().Sectors);

        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 12;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 1;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<SarDbContext>();

        return services;
    }

    public static IServiceCollection AddWhat3Words(
        this IServiceCollection services,
        string? apiKey)
    {
        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            services.Configure<What3WordsOptions>(o => o.ApiKey = apiKey);
            services.AddHttpClient<IWhat3WordsService, What3WordsApiClient>(client =>
                client.BaseAddress = new Uri("https://api.what3words.com/v3/"));
        }
        else
        {
            services.AddSingleton<IWhat3WordsService, StubWhat3WordsService>();
        }

        return services;
    }
}
