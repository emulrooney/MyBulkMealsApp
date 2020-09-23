﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBulkMealsApp.Data;

namespace MyBulkMealsApp.Controllers
{
    public abstract class BaseController<TEntity, TRepository> : Controller
       where TEntity : class, IEntity
       where TRepository : IRepository<TEntity>
    {
        protected readonly TRepository repository;

        public BaseController(TRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await repository.GetAll());
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await repository.Get((int)id);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Recipes/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await repository.Get((int)id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        

        // GET: {controller}/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await repository.Get((int)id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: {controller}/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        protected bool ItemExists(int id)
        {
            return repository.Get(id) != null;
        }

    }
}