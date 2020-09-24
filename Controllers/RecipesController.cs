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
    public class RecipesController : BaseController<Recipe, RecipeRepository>
    {
        
        public RecipesController(RecipeRepository repository) : base(repository)
        {
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemName,ImageUrl,BaseServings,Instructions,Step,Time,Views")] Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                await repository.Add(recipe);
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemName,ImageUrl,BaseServings,Instructions,Step,Time,Views")] Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await repository.Update(recipe);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(recipe.Id))
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
    }
}
