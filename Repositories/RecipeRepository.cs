using Microsoft.EntityFrameworkCore;
using MyBulkApps.Data.EFCore;
using MyBulkMealsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBulkMealsApp.Repositories {
    public class RecipeRepository : EfCoreRepository<Recipe, MyBulkMealsAppContext>
    {
        public RecipeRepository(MyBulkMealsAppContext context) : base(context)
        {
        }

        public async Task<List<Recipe>> GetTopRecipes(int recipesCount)
        {
            var recipes = await context.Recipe.OrderBy(r => r.Views).Take(recipesCount).ToListAsync();
            return recipes;
        }
    }
}
