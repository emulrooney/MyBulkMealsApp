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
        private readonly RecipeRepository _repo;

        public RecipesController(RecipeRepository repo) : base(repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Popular(int pageNumber = 1)
        {
            var list = await PaginatedList<Recipe>.CreateAsync(await repository.GetByViews(true), pageNumber, pageSize);
            return View("Index", list);
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageUrl,BaseServings,Instructions,Time,Views,ItemName,CreatedTime,CreatorId,IsVerified,IsPublic,VerificationSubmissionTime,IsAmendment")] Recipe recipe)
        {
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

        private bool RecipeExists(int id)
        {
            return _repo.Get(id) != null;
        }
    }
}
