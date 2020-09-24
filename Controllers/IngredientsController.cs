using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBulkMealsApp.Data;
using MyBulkMealsApp.Models;
using MyBulkMealsApp.Repositories;

namespace MyBulkMealsApp.Controllers
{
    public class IngredientsController : BaseController<Ingredient, IngredientRepository>
    {

        public IngredientsController(IngredientRepository repository) : base(repository)
        {
        }

        // POST: Ingredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemName,MeasurementType,BaseMeasurement,Calories,Protein,Carbs,Fat")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                await repository.Add(ingredient);
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);
        }

        // POST: Ingredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemName,MeasurementType,BaseMeasurement,Calories,Protein,Carbs,Fat")] Ingredient ingredient)
        {
            if (id != ingredient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.Update(ingredient);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(ingredient.Id))
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
    }
}
