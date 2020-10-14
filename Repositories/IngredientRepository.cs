using Microsoft.EntityFrameworkCore;
using MyBulkApps.Data.EFCore;
using MyBulkMealsApp.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBulkMealsApp.Repositories {
    public class IngredientRepository : BaseRepository<Ingredient, MyBulkMealsAppContext>
    {
        public IngredientRepository(MyBulkMealsAppContext context) : base(context)
        {
        }

        public override async Task<List<Ingredient>> GetAll()
        {
            var ing = context.Set<Ingredient>().OfType<Ingredient>();
            var incInclude = ing.Include(i => i.Measurement);

            return await incInclude.ToListAsync();
        }

        public override async Task<List<Ingredient>> GetByCreationTime(bool descending)
        {
            return await context.Set<Ingredient>().Include(i => i.Measurement).OrderByDescending(e => e.CreatedTime).ToListAsync();
        }

        public async Task<List<Measurement>> GetMeasurements()
        {
            return await context.Measurement.ToListAsync();
        }

    }
}
