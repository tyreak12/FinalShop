using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using FinalShop.Models;    // for ApplicationUser

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider provider)
    {

        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();


        var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();


        string[] roles = { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }


        const string adminEmail = "admin@yourdomain.com";
        const string adminPassword = "P@ssw0rd!";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(adminUser, adminPassword);
            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
 
        }
    }
}
