using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.AdminApp.Controllers
{
    public class TrainingController : Controller
    {
        private readonly ITrainingService _trainingService;
        private readonly ICategoryService _categoryService;

        //constructor to call service

        public TrainingController(ITrainingService trainingService, ICategoryService categoryService)
        {
            _trainingService = trainingService;
            _categoryService = categoryService;
        }

        public IActionResult Trainings()
        {
            var training = _trainingService.GetTraining();
            return View(training);
        }

        public IActionResult CreateTraining()
        {
            List<Category> categories = _categoryService.GetCategory();
            List<CategoryViewModel> categoryViewModels = categories.Select(category => new CategoryViewModel
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
            }).ToList();

            ViewBag.Categories = categoryViewModels;

            return View();
        }
        
        [HttpPost]
        public IActionResult CreateTraining(TrainingViewModel trainingViewModel)
        {
            _trainingService.AddTraining(trainingViewModel);
            return RedirectToAction("Trainings");
        }

        public IActionResult DeleteTraining(TrainingViewModel trainingViewModel)
        {
            bool isDeleted = _trainingService.DeleteTraining(trainingViewModel);
            if (isDeleted)
            {
                return RedirectToAction("Trainings");
            }
            return NotFound();
        }
    }
}
