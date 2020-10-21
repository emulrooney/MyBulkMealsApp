using Microsoft.EntityFrameworkCore;
using MyBulkApps.Data;
using MyBulkMealsApp.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBulkMealsApp.Repositories {
    public class IngredientRepository : BaseRepository<Ingredient, MyBulkMealsAppContext>
    {
        public override IQueryable<Ingredient> Collection { get
            {
                return context.Set<Ingredient>()
                .OfType<Ingredient>()
                .Include(i => i.Measurement)
                .Include(i => i.Creator);
            }
        }

        public IngredientRepository(MyBulkMealsAppContext context) : base(context)
        {
        }

        public override async Task<List<Ingredient>> GetAll()
        {
            return await Collection.ToListAsync();
        }

        public override async Task<List<Ingredient>> GetByCreationTime(bool descending)
        {
            return await Collection.OrderByDescending(e => e.CreatedTime).ToListAsync();
        }

        //TODO Instead, should probably use measurement helper
        public async Task<List<Measurement>> GetMeasurements()
        {
            return await context.Measurement.ToListAsync();
        }

        public async Task<List<Ingredient>> GetAllUnverified()
        {
            return await Collection.OrderByDescending(e => e.CreatedTime).Where(e => !e.IsVerified).ToListAsync();
        }

    }
}
