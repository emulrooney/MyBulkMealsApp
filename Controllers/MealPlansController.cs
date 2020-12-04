using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBulkMealsApp.Models;
using MyBulkMealsApp.Repositories;
using Newtonsoft.Json;

namespace MyBulkMealsApp.Controllers
{
    [Authorize]
    public class MealPlansController : Controller
    {
        private readonly MyBulkMealsAppContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RecipeRepository _recipes;

        public MealPlansController(MyBulkMealsAppContext context, UserManager<ApplicationUser> userManager, RecipeRepository recipes)
        {
            _context = context;
            _userManager = userManager;
            _recipes = recipes;
        }


        // GET: MealPlans
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            return View(await _context.MealPlan.Where(m => m.CreatorId == userId).ToListAsync());
        }

        // GET: MealPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealPlan = await _context.MealPlan
                .Include(m => m.MealPlanEntries)
                .ThenInclude(r => r.Recipe)
                .ThenInclude(r => r.Ingredients)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            var userId = _userManager.GetUserId(User);

            if (mealPlan == null || mealPlan.CreatorId != userId )
            {
                return NotFound();
            }

            return View(mealPlan);
        }

        // GET: MealPlans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MealPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlanName,MealsPerDay,TotalDays,StartDay,EndDay")] MealPlan mealPlan, string recipesList)
        {
            var typeDefinition = new[] { new { Id = 0, Quantity = 0 } };
            var recipes = JsonConvert.DeserializeAnonymousType(recipesList, typeDefinition);

            List<MealPlanEntry> entries = new List<MealPlanEntry>();

            mealPlan.CreatorId = _userManager.GetUserId(User);

            foreach (var r in recipes)
            {
                entries.Add(new MealPlanEntry()
                {
                    RecipeId = r.Id,
                    Quantity = r.Quantity
                });
            }

            if (ModelState.IsValid)
            {
                mealPlan.MealPlanEntries = entries;
                _context.MealPlan.Add(mealPlan);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(mealPlan);
        }

        // GET: MealPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealPlan = await _context.MealPlan.Where(m => m.Id == id)
                .Include(m => m.MealPlanEntries)
                .ThenInclude(m => m.Recipe)
                .ThenInclude(r => r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync();

            var userId = _userManager.GetUserId(User);

            if (mealPlan == null || mealPlan.CreatorId != userId)
            {
                return NotFound();
            }

            ViewData["RecipesList"] = JsonConvert.SerializeObject(
                mealPlan.MealPlanEntries.Select(i => new { i.Recipe, i.Quantity })
                .Select(i => new {
                    label = i.Recipe.ItemName, //assigning as label lets us use default jquery-ui
                    i.Recipe.Id,
                    Calories = i.Recipe.TotalCalories,
                    Protein = i.Recipe.TotalProtein,
                    Carbs = i.Recipe.TotalCarbs,
                    Fat = i.Recipe.TotalFat,
                    i.Quantity
                }).ToList(),
                Formatting.Indented);

            return View(mealPlan);
        }

        // POST: MealPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlanName,CreatorId,MealsPerDay,TotalDays,StartDay,EndDay")] MealPlan mealPlan)
        {
            if (id != mealPlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mealPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MealPlanExists(mealPlan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mealPlan);
        }

        // GET: MealPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealPlan = await _context.MealPlan.Where(m => m.Id == id)
                .Include(m => m.MealPlanEntries)
                .ThenInclude(r => r.Recipe)
                .ThenInclude(r => r.Ingredients)
                .FirstOrDefaultAsync();

            var userId = _userManager.GetUserId(User);

            if (mealPlan == null || mealPlan.CreatorId != userId)
            {
                return NotFound();
            }

            return View(mealPlan);
        }

        // POST: MealPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mealPlan = await _context.MealPlan.FindAsync(id);
            var userId = _userManager.GetUserId(User);

            if (mealPlan == null || mealPlan.CreatorId != userId)
            {
                _context.MealPlan.Remove(mealPlan);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MealPlanExists(int id)
        {
            return _context.MealPlan.Any(e => e.Id == id);
        }

        [Route("MealPlans/AutocompleteRecipes")]
        [Route("MealPlans/Edit/AutocompleteRecipes")]
        public async Task<JsonResult> AutocompleteRecipes(string term)
        {
            var foundRecipes = await _recipes.GetByKeywordWithIngredients(term, 6);

            return Json(foundRecipes
                .Select(i => new {
                label = i.ItemName, //assigning as label lets us use default jquery-ui
                i.Id,
                Calories = i.TotalCalories,
                Protein = i.TotalProtein,
                Carbs = i.TotalCarbs,
                Fat = i.TotalFat
            }).ToArray());
        }


    }
}
