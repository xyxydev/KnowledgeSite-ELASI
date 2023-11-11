using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASI.Basecode.AdminApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        //constructor to call service

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        public IActionResult TrainingCategories()
        {
            var category = _categoryService.GetCategory();
            return View(category);
        }

        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(CategoryViewModel categoryViewModel)
        {
            _categoryService.AddCategory(categoryViewModel);
            return RedirectToAction("TrainingCategories");
        }

        [HttpGet]
        public IActionResult ViewCategory(int id)
        {
            var category = _categoryService.GetCategory(id);
            if (category != null)
            {
                CategoryViewModel categoryViewModel = new()
                {
                    Id = id,
                    CategoryName = category.CategoryName,
                    CategoryDesc = category.CategoryDesc,
                };
                return View(categoryViewModel);
            }
            return NotFound();
        }

        //check category if exist, then display data
        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            var category = _categoryService.GetCategory(id);
            if (category != null)
            {
                CategoryViewModel categoryViewModel = new()
                {
                    Id = id,
                    CategoryName = category.CategoryName,
                    CategoryDesc = category.CategoryDesc,
                };

                return View(categoryViewModel);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult EditCategory(CategoryViewModel categoryViewModel)
        {
            bool isUpdated = _categoryService.UpdateCategory(categoryViewModel);
            if (isUpdated)
            {
                return RedirectToAction("TrainingCategories");
            }
            return NotFound();
        }

        public IActionResult DeleteCategory(CategoryViewModel categoryViewModel)
        {
            bool isDeleted = _categoryService.DeleteCategory(categoryViewModel);
            if (isDeleted)
            {
                return RedirectToAction("TrainingCategories");
            }
            return NotFound();
        }


    }
}
