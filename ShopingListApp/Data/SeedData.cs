using Microsoft.AspNetCore.Identity;

namespace ShopingListApp.Data;

public static class SeedData
{
    public static async Task Initialize(
        UserManager<IdentityUser> userManager, 
        RoleManager<IdentityRole> roleManager
        )
    {
        await SeedRoles(roleManager);
        await SeedUsers(userManager, roleManager);
    }

    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = { "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    public static async Task SeedUsers(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        string userEmail = "admin@gmail.com";
        string userPassword = "Kolegija1@";

        if (await userManager.FindByEmailAsync(userEmail) == null)
        {
            var user = new IdentityUser { 
                UserName = userEmail, 
                Email = userEmail,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(user, userPassword);

            if (!result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
            
        }
    }
}