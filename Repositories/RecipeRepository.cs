using Microsoft.EntityFrameworkCore;
using MyBulkApps.Data.EFCore;
using MyBulkMealsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBulkMealsApp.Repositories {
    public class RecipeRepository : BaseRepository<Recipe, MyBulkMealsAppContext>
    {
        public RecipeRepository(MyBulkMealsAppContext context) : base(context)
        {
        }

        public override async Task<List<Recipe>> GetByKeyword(string keyword)
        {
            return await context.Recipe.Where(r => r.ItemName.Contains(keyword)
            || r.Instructions.Contains(keyword)).ToListAsync();
        }

        public async Task<List<Recipe>> GetByViews(bool descending = true)
        {
            if (descending)
                return await context.Recipe.OrderByDescending(r => r.Views).ToListAsync();
            else
                return await context.Recipe.OrderBy(r => r.Views).ToListAsync();
        }

        public async Task<List<Recipe>> GetTopRecipes(int recipesCount)
        {
            var recipes = await context.Recipe.OrderByDescending(r => r.Views).Take(recipesCount).ToListAsync();
            return recipes;
        }
    }
}
