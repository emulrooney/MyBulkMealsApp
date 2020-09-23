using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBulkMealsApp.Models;
using MyBulkMealsApp.Repositories;

namespace MyBulkMealsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RecipeRepository _recipeRepository;

        public HomeController(ILogger<HomeController> logger, RecipeRepository recipeRepository)
        {            
            _logger = logger;
            _recipeRepository = recipeRepository;
        }

        public async Task<IActionResult> Index()
        {
            var recipes = await _recipeRepository.GetTopRecipes(5);
            ViewData["topRecipes"] = recipes;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
