using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyBulkMealsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBulkMealsApp.Data
{
    public class DbInitializer
    {
        public static AppSecrets AppSecrets { get; set; }


        private static ApplicationUser testAdmin = new ApplicationUser()
        {
            FirstName = "Test",
            LastName = "Admin",
            Location = "Earth",
            UserName = "admin@test.com",
            Email = "admin@test.com",
            PhoneNumber = "111-111-1111",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        private static ApplicationUser testPlayer = new ApplicationUser()
        {
            FirstName = "Demo",
            LastName = "User",
            Location = "Hamilton, ON",
            UserName = "user@test.com",
            Email = "user@test.com",
            PhoneNumber = "222-222-2222",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        public async static Task<string> SeedUsersAndRoles(IServiceProvider serviceProvider)
        {
            string loggedErrors = ""; //Set up - if this changes before returned, something broke

            var context = serviceProvider.GetRequiredService<MyBulkMealsAppContext>();
            context.Database.Migrate();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (roleManager.Roles.Count() > 0)
                loggedErrors += "* Roles already seeded. Skipping seed process.\n";
            else
                loggedErrors += await SeedRoles(roleManager);

            if (userManager.Users.Contains<ApplicationUser>(testAdmin) || userManager.Users.Contains<ApplicationUser>(testPlayer))
                loggedErrors += "* Users already exist. Skipping seed process.\n";
            else
                loggedErrors += await SeedUsers(userManager);

            Console.WriteLine(loggedErrors); //usually easier to inspect w breakpoint, but outputting anyway
            return loggedErrors; //If empty, success!
        }


        private async static Task<string> SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string result = "";

            var adminRole = await roleManager.CreateAsync(new IdentityRole("Admin"));
            //var playerRole = await roleManager.CreateAsync(new IdentityRole("Player"));

            if (!adminRole.Succeeded)
                result += "* Admin role couldn't be created.\n";

            //if (!playerRole.Succeeded)
            //    result += "* Player role couldn't be created.\n";

            return result;
        }

        private async static Task<string> SeedUsers(UserManager<ApplicationUser> userManager)
        {
            string result = "";

            var adminUser = await userManager.CreateAsync(testAdmin, "Test!1"); //AppSecrets.SeededAdminPassword);
            var normalUser = await userManager.CreateAsync(testPlayer, "Test!1");  //AppSecrets.SeededUserPassword);

            var managerUserRole = await userManager.AddToRoleAsync(testAdmin, "Admin");
            //var playerUserRole = await userManager.AddToRoleAsync(testPlayer, "Player");

            if (!adminUser.Succeeded)
                result += "* Admin user couldn't be created.\n";

            if (!normalUser.Succeeded)
                result += "* Regular user couldn't be created.\n";

            if (!managerUserRole.Succeeded)
                result += "* Manager role couldn't be added to manager user.\n";

            //if (!playerUserRole.Succeeded)
            //    result += "* Player role couldn't be added to player user.\n";

            return result;
        }
    }
}
