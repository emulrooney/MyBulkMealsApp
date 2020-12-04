using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBulkMealsApp.Data;
using MyBulkMealsApp.Models;
using MyBulkMealsApp.Repositories;
using Newtonsoft.Json;

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

        public override async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _repo.GetAndView((int)id);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BaseServings,Instructions,Time,Views,ItemName,CreatedTime,IsVerified,IsPublic,IsAmendment")] Recipe recipe, string ingredientsList)
        {
            var typeDefinition = new[] { new { Id = 0, Quantity = 0 } };
            var ingredients = JsonConvert.DeserializeAnonymousType(ingredientsList, typeDefinition);
            
            List<RecipeIngredient> recipeIngredients = new List<RecipeIngredient>();

            foreach (var i in ingredients)
            {
                recipeIngredients.Add(new RecipeIngredient()
                {
                    IngredientId = i.Id,
                    MeasurementAmount = i.Quantity
                });
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            recipe.CreatorId = user.Id;

            if (ModelState.IsValid)
            {
                recipe.Ingredients = recipeIngredients;
                await _repo.Add(recipe);
                var quantity = await _repo.GetNumberOfUnverifiedItems();

                if (quantity > SimpleConfig.VerificationNotificationTrigger && User.IsInRole("Admin"))
                {
                    List<ApplicationUser> users = (List<ApplicationUser>)await _userManager.GetUsersInRoleAsync("Admin");

                    var callbackUrl = Url.Page(
                    "/Admin",
                    pageHandler: null,
                    values: null,
                    protocol: Request.Scheme);

                    await MBMEmailHandler.SendNotificationEmail(recipe, users, callbackUrl, SimpleConfig.VerificationNotificationTrigger);
                }


                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }

        // GET: {controller}/Edit/5
        public override async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _repo.Get((int)id);
            if (recipe == null)
            {
                return NotFound();
            }

            ViewData["IngredientsList"] = JsonConvert.SerializeObject(
                recipe.Ingredients.Select(i => i.Ingredient).Select(i => new
                {
                    label = i.ItemName, //assigning as label lets us use default jquery-ui
                    id = i.Id,
                    calories = i.Calories,
                    protein = i.Protein,
                    carbs = i.Carbs,
                    fat = i.Fat,
                    baseMeasurement = i.BaseMeasurement,
                    currentMeasurement = i.BaseMeasurement,
                    symbol = i.Measurement.Symbol
                }).ToList(),
                Formatting.Indented);
            return View(recipe);
        }


        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BaseServings,Instructions,Time,Views,ItemName,CreatorId,IsVerified,IsPublic,VerificationSubmissionTime,IsAmendment")] Recipe recipe)
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

        [Route("Recipes/AutocompleteIngredients")]
        [Route("Recipes/Edit/AutocompleteIngredients")]
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
                currentMeasurement = i.BaseMeasurement,
                symbol = i.Measurement.Symbol
            }).ToArray());
        }

        private bool RecipeExists(int id)
        {
            return _repo.Get(id) != null;
        }

        public virtual async Task<IActionResult> Report()
        {
            return View();
        }
    }
}
