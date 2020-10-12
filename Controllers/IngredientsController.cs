using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBulkMealsApp.Models;
using MyBulkMealsApp.Repositories;

namespace MyBulkMealsApp.Controllers
{
    public class IngredientsController : BaseController<Ingredient, IngredientRepository>
    {

        public IngredientsController(IngredientRepository repo, UserManager<ApplicationUser> userManager) : base(repo, userManager)
        {
        }

        // POST: Ingredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MeasurementId,BaseMeasurement,Calories,Protein,Carbs,Fat,ItemName,IsPublic")] Ingredient ingredient)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ingredient.CreatorId = user.Id;

            if (ModelState.IsValid)
            {
                await _repo.Add(ingredient);
                return RedirectToAction(nameof(Index));
            }

            return View(ingredient);
        }

        public async override Task<IActionResult> Create()
        {
            ViewData["Measurements"] = await base._repo.GetMeasurements();
            return await base.Create();
        }


        public async override Task<IActionResult> Edit(int? id)
        {
            ViewData["Measurements"] = await base._repo.GetMeasurements();
            return await base.Edit(id);
        }

        // POST: Ingredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MeasurementType,BaseMeasurement,Calories,Protein,Carbs,Fat,ItemName,IsVerified,IsPublic")] Ingredient ingredient)
        {
            if (id != ingredient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.Update(ingredient);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientExists(ingredient.Id))
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
            return View(ingredient);
        }

        private bool IngredientExists(int id)
        {
            return _repo.Get(id) != null;
        }
    }
}
