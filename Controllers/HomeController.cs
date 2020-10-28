using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IngredientRepository _ingredientRepository;

        public HomeController(ILogger<HomeController> logger, RecipeRepository recipeRepository, IngredientRepository ingredientRepository)
        {            
            _logger = logger;
            _recipeRepository = recipeRepository;
            _ingredientRepository = ingredientRepository;
        }

        public async Task<IActionResult> Index()
        {
            var recipes = await _recipeRepository.GetTopRecipes(5);
            ViewData["topRecipes"] = recipes;
            ViewData["recipesCount"] = _recipeRepository.Count().Result;
            ViewData["ingredientsCount"] = _ingredientRepository.Count().Result;

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


        /* ADMIN FUNCTIONALITY */

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            

            ViewData["UnverifiedRecipes"] = await _recipeRepository.GetAllUnverified();
            ViewData["UnverifiedIngredients"] = await _ingredientRepository.GetAllUnverified();

            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> VerifyItem(int id, string itemType)
        {
            switch (itemType)
            {
                case "recipe":
                    await _recipeRepository.Verify(id);
                    break;
                case "ingredient":
                    await _ingredientRepository.Verify(id);
                    break;
                default:
                    return new JsonResult(false);
            }

            return new JsonResult(true);
        }

        public async Task<JsonResult> GetNewRecipesChartData(DateTime? fromDate, DateTime? toDate)
        {
            var chartData = await _recipeRepository.GetNewRecipeChartData(fromDate, toDate, true);
            return new JsonResult(chartData);
        }

        public async Task<JsonResult> GetNewIngredientsChartData(DateTime? fromDate, DateTime? toDate)
        {
            var chartData = await _ingredientRepository.GetNewIngredientChartData(fromDate, toDate, true);
            return new JsonResult(chartData);
        }

        public async Task<JsonResult> GetVerifiedIngredientsChartData(DateTime? fromDate, DateTime? toDate)
        {
            var chartData = await _ingredientRepository.GetVerifiedIngredientsChartData(fromDate, toDate);
            return new JsonResult(chartData);
        }
        public async Task<JsonResult> GetVerifiedRecipesChartData(DateTime? fromDate, DateTime? toDate)
        {
            var chartData = await _recipeRepository.GetVerifiedRecipesChartData(fromDate, toDate);
            return new JsonResult(chartData);
        }

    }
}
