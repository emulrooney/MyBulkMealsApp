using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        public IngredientRepository(MyBulkMealsAppContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }

        public override async Task<List<Ingredient>> GetByCreationTime(bool descending)
        {
            return await Collection.OrderByDescending(e => e.CreatedTime).ToListAsync();
        }

        public override async Task<Ingredient> Verify(int id)
        {
            var ingredient = await context.Set<Ingredient>().FindAsync(id);
            if (ingredient == null)
            {
                return ingredient;
            }

            ingredient.IsVerified = true;
            ingredient.VerificationSubmissionTime = DateTime.Now;

            if (ingredient.IsAmendment)
            {
                var replaced = await Get(id);
                replaced.ItemName = ingredient.ItemName;
                replaced.Calories = ingredient.Calories;
                replaced.Carbs = ingredient.Carbs;
                replaced.Fat = ingredient.Fat;
                replaced.Protein= ingredient.Protein;
                replaced.BaseMeasurement = ingredient.BaseMeasurement;
                replaced.MeasurementId = ingredient.MeasurementId;

                replaced.AmendmentCount--;
                await Delete(ingredient.Id);
                ingredient = replaced;
                await Update(ingredient);
            }

            await context.SaveChangesAsync();

            return ingredient;
        }


        //TODO Instead, should probably use measurement helper
        public async Task<List<Measurement>> GetMeasurements()
        {
            return await context.Measurement.ToListAsync();
        }

        public async Task<List<Ingredient>> GetAllUnverified()
        {
            return await Collection.OrderByDescending(e => e.CreatedTime).Where(e => !e.IsVerified && e.IsPublic).ToListAsync();
        }

        public async Task<List<Ingredient>> GetAllUnverifiedAmendments()
        {
            return await Amendments.OrderByDescending(e => e.CreatedTime).Where(e => !e.IsVerified && e.IsPublic).ToListAsync();
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
                .Select(r => new { Day = r.Day.ToShortDateString(), r.Count})
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