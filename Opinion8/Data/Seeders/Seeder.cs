using System.Collections;
using Microsoft.AspNetCore.Identity;

namespace Opinion8.Data.Seeders;

public static class Seeder
{
    private enum Roles
    {
        Admin,
        Voter
    }

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<
            RoleManager<IdentityRole>
        >();
        UserManager<IdentityUser> userManager = scope.ServiceProvider.GetRequiredService<
            UserManager<IdentityUser>
        >();

        await GenerateRoles(roleManager);
        await GenerateUsers(userManager);
    }

    private static async Task GenerateRoles(RoleManager<IdentityRole> roleManager)
    {
        foreach (Roles role in Enum.GetValues<Roles>())
        {
            if (await roleManager.RoleExistsAsync(role.ToString()))
                continue;

            await roleManager.CreateAsync(new IdentityRole(role.ToString()));
        }
    }

    private static async Task GenerateUsers(UserManager<IdentityUser> userManager)
    {
        Dictionary<Roles, IdentityUser> users =
            new()
            {
                {
                    Roles.Admin,
                    new IdentityUser
                    {
                        UserName = "admin@admin.com",
                        Email = "admin@admin.com",
                        EmailConfirmed = true,
                        PasswordHash = "Admin12345!"
                    }
                },
                {
                    Roles.Voter,
                    new IdentityUser
                    {
                        UserName = "voter@voter.com",
                        Email = "voter@voter.com",
                        EmailConfirmed = true,
                        PasswordHash = "Voter12345!"
                    }
                }
            };
        foreach (KeyValuePair<Roles, IdentityUser> kvp in users)
        {
            IdentityUser? userName = await userManager.FindByNameAsync(kvp.Value.UserName ?? "");
            if (userName != null)
                continue;

            IdentityUser? user = await userManager.FindByEmailAsync(kvp.Value.Email ?? "");
            if (user != null)
                continue;

            await userManager.CreateAsync(kvp.Value, kvp.Value.PasswordHash ?? "");
            await userManager.AddToRoleAsync(kvp.Value, kvp.Key.ToString());
        }
    }

    public static async Task SeedDatabaseAsync(IServiceProvider services)
    {
        using IServiceScope scope = services.CreateScope();
        IServiceProvider serviceProvider = scope.ServiceProvider;

        try
        {
            await InitializeAsync(serviceProvider);
        }
        catch (Exception ex)
        {
            ILogger<Program> logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred seeding the database.");
        }
    }
}
