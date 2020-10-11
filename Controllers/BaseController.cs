using System;
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
        protected int pageSize = 20; //temp

        public BaseController(TRepository repository)
        {
            this.repository = repository;
        }

        /* Types of Index Page */

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var list = await PaginatedList<TEntity>.CreateAsync(await repository.GetAll(), pageNumber, pageSize);
            return View(list);
        }

        public async Task<IActionResult> Results(string keyword, int pageNumber = 1)
        {
            var list = await PaginatedList<TEntity>.CreateAsync(await repository.GetByKeyword(keyword), pageNumber, pageSize);
            return View("Index", list);
        }

        public async Task<IActionResult> Newest(int pageNumber = 1)
        {
            var list = await PaginatedList<TEntity>.CreateAsync(await repository.GetByCreationTime(true), pageNumber, pageSize);
            ViewData["showCreatedTime"] = true;
            return View("Index", list);
        } 

        public async Task<IActionResult> Random(int quantity)
        {
            var list = await PaginatedList<TEntity>.CreateAsync(await repository.GetRandom(quantity), 1, 20);
            return View("Index", list);
        }

        /* Individual Pages */

        // GET: {controller}/Details/5
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

        // GET: {controller}/Create
        public async virtual Task<IActionResult> Create()
        {
            return View();
        }

        // GET: {controller}/Edit/5
        public async virtual Task<IActionResult> Edit(int? id)
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