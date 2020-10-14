using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyBulkMealsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            EmailConfirmed = true
        };

        private static ApplicationUser testUser = new ApplicationUser()
        {
            FirstName = "Demo",
            LastName = "User",
            Location = "Hamilton, ON",
            UserName = "user@test.com",
            Email = "user@test.com",
            PhoneNumber = "222-222-2222",
            EmailConfirmed = true
        };

        private static Measurement grams = new Measurement()
        {
            Name = "Grams",
            Symbol = "g"
        };

        private static Measurement millilitres = new Measurement()
        {
            Name = "Millilitres",
            Symbol = "ml"
        };

        private static Measurement kilograms = new Measurement()
        {
            Name = "Kilograms",
            Symbol = "kg"
        };
      
        private static Measurement litres = new Measurement()
        {
            Name = "Litres",
            Symbol = "l"
        };

        public async static Task<string> SeedDatabase(IServiceProvider serviceProvider)
        {
            string loggedErrors = ""; //Set up - if this changes before returned, something broke

            var context = serviceProvider.GetRequiredService<MyBulkMealsAppContext>();
            context.Database.Migrate();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var measurementHandler = serviceProvider.GetService<MeasurementHandler>();

            if (roleManager.Roles.Count() > 0)
                loggedErrors += "* Roles already seeded. Skipping seed process.\n";
            else
                loggedErrors += await SeedRoles(roleManager);

            if (userManager.Users.FirstOrDefault<ApplicationUser>(u => u.UserName == testAdmin.UserName) != null &&
                userManager.Users.FirstOrDefault<ApplicationUser>(u => u.UserName == testUser.UserName) != null)
                loggedErrors += "* Users already exist. Skipping seed process.\n";
            else
                loggedErrors += await SeedUsers(userManager);

            if (!measurementHandler.HasMeasurements())
                loggedErrors += await SeedMeasurements(measurementHandler);
            else
                loggedErrors += "* Already seeded measurements.\n";

            Console.WriteLine(loggedErrors); //usually easier to inspect w breakpoint, but outputting anyway
            return loggedErrors; //If empty, success!
        }

        private async static Task<string> SeedMeasurements(MeasurementHandler measurements)
        {
            String seeded = "";

            var g = await measurements.AddMeasurementType(grams);
            var ml = await measurements.AddMeasurementType(millilitres);
            var kg = await measurements.AddMeasurementType(kilograms);
            var l = await measurements.AddMeasurementType(litres);

            if (g == null)
                seeded += "* 'Grams' already in database.\n";
            if (ml == null)
                seeded += "* 'Millilitres' already in database.\n";
            if (kg == null)
                seeded += "* 'Kilograms' already in database.\n";
            if (l == null)
                seeded += "* 'Litres' already in database.";

            return seeded;
        }


        private async static Task<string> SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string result = "";

            var adminRole = await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!adminRole.Succeeded)
                result += "* Admin role couldn't be created.\n";

            return result;
        }

        private async static Task<string> SeedUsers(UserManager<ApplicationUser> userManager)
        {
            string result = "";

            var adminUser = await userManager.CreateAsync(testAdmin, AppSecrets.SeededAdminPassword);
            var normalUser = await userManager.CreateAsync(testUser, AppSecrets.SeededUserPassword);

            var managerUserRole = await userManager.AddToRoleAsync(testAdmin, "Admin");

            if (!adminUser.Succeeded)
                result += "* Admin user couldn't be created.\n";

            if (!normalUser.Succeeded)
                result += "* Regular user couldn't be created.\n";

            if (!managerUserRole.Succeeded)
                result += "* Manager role couldn't be added to manager user.\n";

            return result;
        }
    }
}
