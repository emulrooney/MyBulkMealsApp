﻿using Microsoft.EntityFrameworkCore;
using MyBulkApps.Data.EFCore;
using MyBulkMealsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBulkMealsApp.Repositories {
    public class RecipeRepository : BaseRepository<Recipe, MyBulkMealsAppContext>
    {
        public override IQueryable<Recipe> Collection
        {
            get
            {
                return context.Set<Recipe>()
                .OfType<Recipe>()
                .Include(i => i.Creator);
            }
        }

        public RecipeRepository(MyBulkMealsAppContext context) : base(context)
        {
        }

        public async Task<Recipe> GetAndView(int id)
        {
            var recipe = await Get(id);
            recipe.Views++;
            await context.SaveChangesAsync();

            return recipe;
        }

        public override async Task<Recipe> Get(int id)
        {
            return await Collection
                .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
                .ThenInclude(i => i.Measurement)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public override async Task<List<Recipe>> GetByKeyword(string keyword)
        {
            return await Collection
                .Where(r => r.ItemName.Contains(keyword) || r.Instructions.Contains(keyword)).ToListAsync();
        }

        public async Task<List<Recipe>> GetByViews(bool descending = true)
        {
            if (descending)
                return await Collection.OrderByDescending(r => r.Views).ToListAsync();
            else
                return await Collection.OrderBy(r => r.Views).ToListAsync();
        }

        public async Task<List<Recipe>> GetTopRecipes(int recipesCount)
        {
            var recipes = await Collection.OrderByDescending(r => r.Views).Take(recipesCount).ToListAsync();
            return recipes;
        }

        public async Task<List<Recipe>> GetAllUnverified()
        {
            return await Collection.OrderByDescending(e => e.CreatedTime).Where(e => !e.IsVerified).ToListAsync();
        }
    }
}
