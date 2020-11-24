using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBulkMealsApp.Models;

namespace MyBulkMealsApp.Controllers
{
    [Authorize]
    public class MealPlansController : Controller
    {
        private readonly MyBulkMealsAppContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MealPlansController(MyBulkMealsAppContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: MealPlans
        public async Task<IActionResult> Index()
        {
            return View(await _context.MealPlan.Where(m => MealPlanIsCurrentUsers(m)).ToListAsync());
        }

        // GET: MealPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealPlan = await _context.MealPlan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mealPlan == null || !MealPlanIsCurrentUsers(mealPlan) )
            {
                return NotFound();
            }

            return View(mealPlan);
        }

        // GET: MealPlans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MealPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlanName,CreatorId,MealsPerDay,TotalDays,StartDay,EndDay")] MealPlan mealPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mealPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mealPlan);
        }

        // GET: MealPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealPlan = await _context.MealPlan.FindAsync(id);
            if (mealPlan == null || !MealPlanIsCurrentUsers(mealPlan))
            {
                return NotFound();
            }
            return View(mealPlan);
        }

        // POST: MealPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlanName,CreatorId,MealsPerDay,TotalDays,StartDay,EndDay")] MealPlan mealPlan)
        {
            if (id != mealPlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mealPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MealPlanExists(mealPlan.Id))
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
            return View(mealPlan);
        }

        // GET: MealPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealPlan = await _context.MealPlan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mealPlan == null || !MealPlanIsCurrentUsers(mealPlan))
            {
                return NotFound();
            }

            return View(mealPlan);
        }

        // POST: MealPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mealPlan = await _context.MealPlan.FindAsync(id);
            if (MealPlanIsCurrentUsers(mealPlan))
            {
                _context.MealPlan.Remove(mealPlan);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MealPlanExists(int id)
        {
            return _context.MealPlan.Any(e => e.Id == id);
        }

        private bool MealPlanIsCurrentUsers(MealPlan mp)
        {
            var userId = _userManager.GetUserId(User);
            return mp.CreatorId == userId;
        }
    }
}
