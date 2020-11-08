using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBulkMealsApp.Data;
using MyBulkMealsApp.Models;

namespace MyBulkMealsApp.Controllers
{
    [Authorize]
    public abstract class BaseController<TEntity, TRepository> : Controller
       where TEntity : class, IEntity
       where TRepository : IRepository<TEntity>
    {
        protected readonly TRepository _repo;
        protected readonly UserManager<ApplicationUser> _userManager;

        protected int pageSize = 20; //temp

        public BaseController(TRepository repository, UserManager<ApplicationUser> userManager)
        {
            this._repo = repository;
            this._userManager = userManager;
        }

        /* Types of Index Page */

        [AllowAnonymous]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var list = await PaginatedList<TEntity>.CreateAsync(await _repo.GetAll(_userManager.GetUserAsync(User).Result), pageNumber, pageSize);
            return View(list);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Results(string keyword, int pageNumber = 1)
        {
            var list = await PaginatedList<TEntity>.CreateAsync(await _repo.GetByKeyword(keyword), pageNumber, pageSize);
            return View("Index", list);
        }

        public async Task<IActionResult> Newest(int pageNumber = 1)
        {
            var list = await PaginatedList<TEntity>.CreateAsync(await _repo.GetByCreationTime(true), pageNumber, pageSize);
            ViewData["showCreatedTime"] = true;
            return View("Index", list);
        } 

        public async Task<IActionResult> Created()
        {
            var created = await _repo.GetCreatedBy(_userManager.GetUserAsync(User).Result.Id);
            var list = await PaginatedList<TEntity>.CreateAsync(created, 1, 20);
            return View("Index", list);
        }

        public async Task<IActionResult> Saved()
        {
            var created = await _repo.GetSavedBy(_userManager.GetUserAsync(User).Result.Id);
            var list = await PaginatedList<TEntity>.CreateAsync(created, 1, 20);
            return View("Index", list);
        }

        public async Task<IActionResult> Random(int quantity)
        {
            var list = await PaginatedList<TEntity>.CreateAsync(await _repo.GetRandom(quantity), 1, 20);
            return View("Index", list);
        }

        /* Individual Pages */

        // GET: {controller}/Details/5
        public virtual async Task<IActionResult> Details(int? id)
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

            var recipe = await _repo.Get((int)id);
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

            var recipe = await _repo.Get((int)id);
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
            await _repo.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        protected bool ItemExists(int id)
        {
            return _repo.Get(id) != null;
        }

    }
}