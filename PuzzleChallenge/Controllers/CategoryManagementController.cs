using Microsoft.AspNetCore.Mvc;
using PuzzleChallenge.AppService;
using PuzzleChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuzzleChallenge.Controllers
{
    public class CategoryManagementController : Controller
    {
        private readonly CategoriesAppService _categoriesAppService;

        public CategoryManagementController(CategoriesAppService categoriesAppService)
        {
            _categoriesAppService = categoriesAppService;
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category request)
        {
            var cat = new Category
            {
                Name = request.Name,
                RoleAuthorized = request.RoleAuthorized
            };

            await _categoriesAppService.AddCategory(cat);

            return RedirectToAction("Index", "Home");
        }
    }
}
