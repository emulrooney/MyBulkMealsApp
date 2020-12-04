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
using MyBulkMealsApp.Repositories;
using Newtonsoft.Json;

namespace MyBulkMealsApp.Controllers
{
    /// <summary>
    /// Minimal controller for user accounts (as ApplicationUser).
    /// </summary>
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MyBulkMealsAppContext _context;

        public AccountsController(UserManager<ApplicationUser> userManager, MyBulkMealsAppContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Accounts/Details/id
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Accounts/ID
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FirstName, LastName, Location")] ApplicationUser user)
        {
            var userId = _userManager.GetUserAsync(User).Result.Id;
            var foundUser = await _userManager.FindByIdAsync(id);


            if (id != userId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    foundUser.FirstName = user.FirstName;
                    foundUser.LastName = user.LastName;
                    foundUser.Location = user.Location;

                    await _userManager.UpdateAsync(foundUser);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View(user);
            }

            return View(user);
        }
        public bool UserExists(string userId) 
        {
            return true;
        }
 
    }
}
