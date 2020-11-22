using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public async Task<IActionResult> Create([Bind("MeasurementId,BaseMeasurement,CreatorId,Calories,Protein,Carbs,Fat,ItemName,IsPublic")] Ingredient ingredient)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ingredient.CreatorId = userId;

            if (ModelState.IsValid)
            {
                await _repo.Add(ingredient);
                return RedirectToAction(nameof(Index));
            }

            ViewData["Measurements"] = await base._repo.GetMeasurements();
            return View(ingredient);
        }


        // POST: Ingredients/Amend
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Amend([Bind("MeasurementId,BaseMeasurement,Calories,Protein,Carbs,Fat,ItemName")] Ingredient ingredient, int id)
        {
            ingredient.IsAmendment = true;
            ingredient.BasedOn = id;
            ingredient.CreatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ingredient.IsPublic = true;

            return await Create(ingredient);
        }
        
        public async override Task<IActionResult> Create()
        {
            ViewData["Measurements"] = await base._repo.GetMeasurements();
            return await base.Create();
        }

        public async override Task<IActionResult> Amend(int id)
        {
            ViewData["Measurements"] = await base._repo.GetMeasurements();
            return await base.Amend(id);
        }

        public async override Task<IActionResult> Edit(int? id)
        {
            ViewData["Measurements"] = await base._repo.GetMeasurements();
            return await base.Edit(id);
        }

        public async override Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                var item = await _repo.Get((int)id);
                return await base.Results(item.ItemName, 1);
            }

            return await Index(1);
        }

        // POST: Ingredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BaseMeasurement,Calories,Protein,Carbs,Fat,ItemName,IsVerified,IsPublic")] Ingredient ingredient, int measurementId)
        {
            if (id != ingredient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ingredient.MeasurementId = measurementId;
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
