using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyBulkMealsApp.Models;
using MyBulkMealsApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBulkMealsApp.Data
{
    public class DbInitializer
    {
        public static AppSecrets AppSecrets { get; set; }

        private static readonly ApplicationUser testAdmin = new ApplicationUser()
        {
            FirstName = "Test",
            LastName = "Admin",
            Location = "Earth",
            UserName = "admin@test.com",
            Email = "admin@test.com",
            PhoneNumber = "111-111-1111",
            EmailConfirmed = true
        };

        private static readonly ApplicationUser testUser = new ApplicationUser()
        {
            FirstName = "Demo",
            LastName = "User",
            Location = "Hamilton, ON",
            UserName = "user@test.com",
            Email = "user@test.com",
            PhoneNumber = "222-222-2222",
            EmailConfirmed = true
        };

        private static readonly Measurement grams = new Measurement()
        {
            Name = "Grams",
            Symbol = "g"
        };

        private static readonly Measurement millilitres = new Measurement()
        {
            Name = "Millilitres",
            Symbol = "ml"
        };

        private static readonly Measurement kilograms = new Measurement()
        {
            Name = "Kilograms",
            Symbol = "kg"
        };
      
        private static readonly Measurement litres = new Measurement()
        {
            Name = "Litres",
            Symbol = "l"
        };

        private static readonly Measurement unit = new Measurement()
        {
            Name = "Unit",
            Symbol = ""
        };


        private static readonly string recipeFilepath = "Data/UserItemSeedFiles/recipes.txt";
        private static readonly string ingredientFilepath = "Data/UserItemSeedFiles/ingredients.txt";


        public async static Task<string> SeedDatabase(IServiceProvider serviceProvider)
        {
            string loggedErrors = ""; //Set up - if this changes before returned, something broke

            var context = serviceProvider.GetRequiredService<MyBulkMealsAppContext>();
            context.Database.Migrate();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            
            var measurementHandler = serviceProvider.GetService<MeasurementHandler>();
            var ingredientRepo = serviceProvider.GetService<IngredientRepository>();
            var recipeRepo = serviceProvider.GetService<RecipeRepository>();

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

            if (await ingredientRepo.Count() == 0)
                loggedErrors += await SeedIngredients(ingredientRepo);
            else
                loggedErrors += "* Already have ingredients in DB. Skipping seed process. \n";

            if (await recipeRepo.Count() == 0)
                loggedErrors += await SeedRecipes(recipeRepo);
            else
                loggedErrors += "* Already have recipes in DB. Skipping seed process. \n";

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
            var u = await measurements.AddMeasurementType(unit);

            if (g == null)
                seeded += "* 'Grams' already in database.\n";
            if (ml == null)
                seeded += "* 'Millilitres' already in database.\n";
            if (kg == null)
                seeded += "* 'Kilograms' already in database.\n";
            if (l == null)
                seeded += "* 'Litres' already in database.\n";
            if (u == null)
                seeded += "* 'Units' already in database.\n";

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

        private async static Task<string> SeedRecipes(RecipeRepository repo)
        {
            string result = "";
            string [] recipes = System.IO.File.ReadAllLines(recipeFilepath);
            List<Recipe> recipeList = new List<Recipe>();

            foreach (var recipe in recipes)
            {
                var r = recipe.Split("|");
                try
                {
                    var addedRecipe = new Recipe
                    {
                        Id = 0,
                        ItemName = r[0],
                        BaseServings = Int32.Parse(r[1]),
                        Instructions = r[2],
                        Time = Int32.Parse(r[3]),
                        Ingredients = new List<RecipeIngredient>()
                    };

                    int[] ingredients = r[4].Split(",").Select(i => Int32.Parse(i)).ToArray();
                    double[] quantities = r[5].Split(",").Select(i => double.Parse(i)).ToArray();

                    for (int i = 0; i < ingredients.Length && i < quantities.Length; i++)
                    {
                        addedRecipe.Ingredients.Add(new RecipeIngredient { Id = 0, IngredientId = ingredients[i], MeasurementAmount = quantities[i]});
                    }

                    recipeList.Add(addedRecipe);
                }
                catch (Exception e)
                {
                    result += "* Recipe Seed Failed: Couldn't parse line: " + recipe + "\n";
                }
            }

            await repo.Add(recipeList);

            return result;
        }

            private async static Task<string> SeedIngredients(IngredientRepository repo)
        {
            string result = "";

            //Filereader
            string[] ingredients = System.IO.File.ReadAllLines(ingredientFilepath);
            List<Ingredient> ingredientList = new List<Ingredient>();
            
            foreach (var ingredient in ingredients)
            {
                var i = ingredient.Split("|");

                try
                {
                    ingredientList.Add(new Ingredient {
                        Id = 0,
                        ItemName = i[0],
                        MeasurementId = Int32.Parse(i[1]), 
                        BaseMeasurement = Int32.Parse(i[2]), 
                        Calories = short.Parse(i[3]), 
                        Protein = short.Parse(i[4]), 
                        Carbs = short.Parse(i[5]), 
                        Fat = short.Parse(i[6]), 
                        IsVerified = true
                    });
                } 
                catch (Exception e)
                {
                    result += "* Ingredient Seed Failed: Couldn't parse line: " + ingredient + "\n";
                }
            }

            await repo.Add(ingredientList);

            return result;
        }

    }
}
