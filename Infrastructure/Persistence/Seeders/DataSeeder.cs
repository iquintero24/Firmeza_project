

using Firmeza.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Firmeza.Infrastructure.Persistence.Seeders;

public static class DataSeeder
{
    // Método principal para ejecutar desde Program.cs
    public static async Task SeedRolesAndAdminUserAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await EnsureRoleExists(roleManager, "Administrator");
        await EnsureRoleExists(roleManager, "User");

        await EnsureAdminUserExists(userManager);
    }

    private static async Task EnsureRoleExists(RoleManager<IdentityRole> roleManager, string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    private static async Task EnsureAdminUserExists(UserManager<ApplicationUser> userManager)
    {
        const string adminEmail = "admin@firmeza.com";
        const string adminPassword = "Admin123!";

        var adminUser = await userManager.FindByNameAsync(adminEmail);

        if (adminUser == null)
        {
            var newAdminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(newAdminUser, adminPassword);

            if (result.Succeeded)
                await userManager.AddToRoleAsync(newAdminUser, "Administrator");
        }
    }
}