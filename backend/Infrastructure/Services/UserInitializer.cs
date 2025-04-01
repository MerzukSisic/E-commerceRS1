using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class UserInitializer(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
{
    public async Task InitializeAsync()
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
        }
        if (!await roleManager.RoleExistsAsync("Customer"))
        {
            await roleManager.CreateAsync(new IdentityRole { Name = "Customer" });
        }

        if (await userManager.FindByNameAsync("admin@test.com") == null)
        {
            var adminUser = new AppUser
            {
                UserName = "admin@test.com",
                Email = "admin@test.com",
            };

            await userManager.CreateAsync(adminUser, "Pa$$w0rd");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        if (await userManager.FindByNameAsync("user1@test.com") == null)
        {
            var user1 = new AppUser
            {
                UserName = "user1@test.com",
                Email = "user1@test.com",
                FirstName = "User",
                LastName = "First",
                EmailConfirmed = true 

            };

            var result1 = await userManager.CreateAsync(user1, "Pa$$w0rd");
            if (result1.Succeeded)
            {
                await userManager.AddToRoleAsync(user1, "Customer");
            }
            else
            {
                foreach (var error in result1.Errors)
                {
                    Console.WriteLine($"Identity Error (user1): {error.Description}");
                }
            }
        }

        if (await userManager.FindByNameAsync("user2@test.com") == null)
        {
            var user2 = new AppUser
            {
                UserName = "user2@test.com",
                Email = "user2@test.com",
                FirstName = "User",
                LastName = "Second"
            };

            var result2 = await userManager.CreateAsync(user2, "Pa$$w0rd");
            if (result2.Succeeded)
            {
                await userManager.AddToRoleAsync(user2, "Customer");
            }
            else
            {
                foreach (var error in result2.Errors)
                {
                    Console.WriteLine($"Identity Error (user2): {error.Description}");
                }
            }
        }
        
    }
}