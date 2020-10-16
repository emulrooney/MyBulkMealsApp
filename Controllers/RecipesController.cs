using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBulkMealsApp.Data;
using MyBulkMealsApp.Models;
using MyBulkMealsApp.Repositories;

namespace MyBulkMealsApp.Controllers
{

    public class RecipesController : BaseController<Recipe, RecipeRepository>
    {
        private readonly IngredientRepository ingredients;

        public RecipesController(RecipeRepository repo, IngredientRepository ingredients, UserManager<ApplicationUser> userManager) : base(repo, userManager)
        {
            this.ingredients = ingredients;
        }

        public async Task<IActionResult> Popular(int pageNumber = 1)
        {
            var list = await PaginatedList<Recipe>.CreateAsync(await base._repo.GetByViews(true), pageNumber, pageSize);
            return View("Index", list);
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageUrl,BaseServings,Instructions,Time,Views,ItemName,CreatedTime,IsVerified,IsPublic,IsAmendment")] Recipe recipe, string recipeIds)
        {
            

            var user = await _userManager.GetUserAsync(HttpContext.User);
            recipe.CreatorId = user.Id;

            if (ModelState.IsValid)
            {
                await _repo.Add(recipe);

                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImageUrl,BaseServings,Instructions,Time,Views,ItemName,CreatorId,IsVerified,IsPublic,VerificationSubmissionTime,IsAmendment")] Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.Update(recipe);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
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
            return View(recipe);
        }

        public async Task<JsonResult> AutocompleteIngredients(string term) 
        {
            var foundIngredients = await ingredients.GetByKeyword(term, 6);

            return Json(foundIngredients.Select(i => new { 
                label = i.ItemName, //assigning as label lets us use default jquery-ui
                i.Id,
                i.Calories,
                i.Protein,
                i.Carbs,
                i.Fat,
                i.BaseMeasurement,
                i.DefaultMeasurement
            }).ToArray());
        }

        private bool RecipeExists(int id)
        {
            return _repo.Get(id) != null;
        }
    }
}
