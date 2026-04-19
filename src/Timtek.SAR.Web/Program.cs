using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TA.Utils.Core.Diagnostics;
using Timtek.SAR.Application.CaseManagement;
using Timtek.SAR.Application.OrganisationManagement;
using Timtek.SAR.Application.UserManagement;
using Timtek.SAR.Domain.Entities;
using Timtek.SAR.Infrastructure.DependencyInjection;
using Timtek.SAR.Infrastructure.Persistence;
using Timtek.SAR.Web.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<ILog>(new DegenerateLoggerService());
builder.Services.AddSarInfrastructure(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SarDatabase") ?? "Data Source=sar.db"));
builder.Services.AddScoped<IOrganisationService, OrganisationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICaseService, CaseService>();

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme, options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/access-denied";
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Apply pending migrations and seed data in development
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SarDbContext>();
    if (app.Environment.IsDevelopment())
    {
        await db.Database.MigrateAsync();

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
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
