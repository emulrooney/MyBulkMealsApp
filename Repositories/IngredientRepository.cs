using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MyBulkApps.Data;
using MyBulkMealsApp.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBulkMealsApp.Repositories
{
    public class IngredientRepository : BaseRepository<Ingredient, MyBulkMealsAppContext>
    {
        public override IQueryable<Ingredient> Collection
        {
            get
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

        [Authorize("Admin")]
        public async Task<dynamic[]> GetNewIngredientChartData(DateTime? fromDate = null, DateTime? toDate = null, bool verified = true)
        {
            DateTime from = fromDate ?? DateTime.Now.AddDays(-30);
            DateTime to = toDate ?? DateTime.Now;

            var ingredients = await Collection
                .Where(r => r.IsVerified || verified)
                .GroupBy(r => r.CreatedTime.Date)
                .Select(r => new { Day = r.Key, Count = r.Count() })
                .Where(r => r.Day >= from && r.Day <= to)
                .OrderBy(r => r.Day)
                .ToArrayAsync();

            return ingredients;
        }

        public async Task<dynamic[]> GetVerifiedIngredientsChartData(DateTime? fromDate = null, DateTime? toDate = null)
        {
            DateTime from = fromDate ?? DateTime.Now.AddDays(-30);
            DateTime to = toDate ?? DateTime.Now;

            var ingredients = await Collection
                .Where(r => r.CreatedTime >= from && r.CreatedTime <= to)
                .GroupBy(r => r.IsVerified)
                .Select(r => new { verified = (r.Key ? "Verified" : "Unverified"), count = r.Count() })
                .ToArrayAsync();

            return ingredients;
        }
    }

}