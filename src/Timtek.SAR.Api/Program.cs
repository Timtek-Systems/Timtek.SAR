using Timtek.SAR.Api.Endpoints;
using Timtek.SAR.Api.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TA.Utils.Core.Diagnostics;
using Timtek.Patterns.DataAccess.EFCore;
using Timtek.SAR.Application.OrganisationManagement;
using Timtek.SAR.Application.UserManagement;
using Timtek.SAR.Domain.Entities;
using Timtek.SAR.Infrastructure.DependencyInjection;
using Timtek.SAR.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<ILog>(new DegenerateLoggerService());
builder.Services.AddSarInfrastructure(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SarDatabase") ?? "Data Source=sar.db"));
builder.Services.AddScoped<IOrganisationService, OrganisationService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme, options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/access-denied";
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
});

var app = builder.Build();

// Apply pending migrations in development; check in production
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SarDbContext>();
    var log = scope.ServiceProvider.GetRequiredService<ILog>();
    if (app.Environment.IsDevelopment())
    {
        await db.Database.MigrateAsync();

        // Seed a default organisation if none exists
        if (!db.Organisations.Any())
        {
            db.Organisations.Add(new Timtek.SAR.Domain.Entities.Organisation
            {
                Id = Guid.NewGuid(),
                Name = "Default Organisation",
            });
            await db.SaveChangesAsync();
        }

        // Seed roles
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        string[] roles = ["Administrator", "CaseManager", "TeamLead", "DroneOperator", "GroundSearcher"];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new ApplicationRole(role));
        }
    }
    else
    {
        await new MigrationChecker(db, log).ThrowIfPendingMigrationsAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapHealthChecks("/health");
app.MapOrganisationEndpoints();
app.MapAuthEndpoints();
app.MapUserEndpoints();

app.Run();
