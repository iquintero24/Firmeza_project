using Microsoft.AspNetCore.Identity;

namespace Firmeza.web.Data.Seeders;

public class DataSeeder
{
    // method main call in Program.cs
    public static async Task SeedRolesAndAdminUserAsync(IServiceProvider serviceProvider)
    {
        // Get instances of RoleManager and UserManager
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        // define roles in the system
        await EnsureRoleExists(roleManager, "Administrator");
        await EnsureRoleExists(roleManager, "User");
        
        // create default admin user (if not exists)
        await EnsureAdminUserExists(userManager);
    }
    
    // method to ensure a role exists
    private static async Task EnsureRoleExists(RoleManager<IdentityRole> roleManager, string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            // create a new IndentityRole con name roleName
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
    
    // method for create user and admin and define role
    private static async Task EnsureAdminUserExists(UserManager<ApplicationUser> userManager)
    {
        const string adminEmail = "admin@firmeza.com";
        const string adminPassword = "Admin123!";
        
        // check if the admin user already exists
        var adminUser = await userManager.FindByNameAsync(adminEmail);

        if (adminUser == null)
        {
            var newAdminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            
            // create the admin user with the specified password
            var result = await userManager.CreateAsync(newAdminUser, adminPassword);
            
            // if the user is created successfully, assign the Administrator role
            if (result.Succeeded) await userManager.AddToRoleAsync(newAdminUser, "Administrator");
        }
        
    }
}