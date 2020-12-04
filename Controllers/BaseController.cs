using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        protected int pageSize = SimpleConfig.ItemsPerPage;

        public BaseController(TRepository repository, UserManager<ApplicationUser> userManager)
        {
            this._repo = repository;
            this._userManager = userManager;
        }

        /* Types of Index Page */

        [AllowAnonymous]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var user = _userManager.GetUserAsync(User).Result;

            var list = await PaginatedList<TEntity>.CreateAsync(await _repo.GetAll(user), pageNumber, pageSize);
            ViewData["SavedIds"] = await _repo.GetSavedIds(list);

            return View(list);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Results(string keyword, int pageNumber = 1)
        {
            var list = await PaginatedList<TEntity>.CreateAsync(await _repo.GetByKeyword(keyword), pageNumber, pageSize);
            ViewData["SavedIds"] = await _repo.GetSavedIds(list);

            return View("Index", list);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Newest(int pageNumber = 1)
        {
            var list = await PaginatedList<TEntity>.CreateAsync(await _repo.GetByCreationTime(true), pageNumber, pageSize);
            ViewData["SavedIds"] = await _repo.GetSavedIds(list);
            ViewData["showCreatedTime"] = true;
            return View("Index", list);
        } 

        public async Task<IActionResult> Created(int pageNumber = 1)
        {
            var created = await _repo.GetCreatedBy(_userManager.GetUserAsync(User).Result.Id);
            var list = await PaginatedList<TEntity>.CreateAsync(created, pageNumber, pageSize);
            return View("Index", list);
        }

        public async Task<IActionResult> Saved(int pageNumber = 1)
        {
            List<TEntity> created = _repo.GetSavedBy(_userManager.GetUserAsync(User).Result.Id);

            var list = await PaginatedList<TEntity>.CreateAsync(created, pageNumber, pageSize);
            return View("Index", list);
        }

        public async Task<IActionResult> Amendments(int id, int pageNumber = 1)
        {
            var amendments = await _repo.GetAmendmentsFor(id);
            return View("Index", await PaginatedList<TEntity>.CreateAsync(amendments, pageNumber, pageSize));
        }

        public virtual async Task<JsonResult> SaveItem(int id)
        {
            var saved = await _repo.ToggleSavedItem(id, _userManager.GetUserAsync(User).Result.Id);
            return new JsonResult(saved);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Random(int quantity)
        {
            var list = await PaginatedList<TEntity>.CreateAsync(await _repo.GetRandom(quantity), 1, quantity);
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

        // GET: {controller}/Amend
        public async virtual Task<IActionResult> Amend(int id)
        {
            var item = await _repo.Get(id);
            return View(item);
        }

        // GET: {controller}/Copy
        public async virtual Task<IActionResult> Copy(int id)
        {
            var item = await _repo.Get(id);
            return View(item);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Copy([Bind("MeasurementId,BaseMeasurement,Calories,Protein,Carbs,Fat,ItemName,IsPublic")] Ingredient ingredient, int id)
        {
            var entity = await _repo.Get(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var other = entity.Copy(id);

            other.AmendmentCount = 0;
            other.CreatorId = userId;

            await _repo.Add((TEntity)other);

            await _repo.ToggleSavedItem(other.Id, userId);

            return await Saved();
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