using Microsoft.EntityFrameworkCore;
using MyBulkApps.Data.EFCore;
using MyBulkMealsApp.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBulkMealsApp.Repositories {
    public class IngredientRepository : EfCoreRepository<Ingredient, MyBulkMealsAppContext>
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

        public async Task<List<Measurement>> GetMeasurements()
        {
            return await context.Measurement.ToListAsync();
        }

    }
}
