using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly ITrainingRepository _trainingRepository;

        public TrainingService(ITrainingRepository trainingRepository)
        {
            _trainingRepository = trainingRepository;
        }
        public void AddTraining(TrainingViewModel trainingViewModel)
        {
            Training training = new()
            {
                Id = trainingViewModel.Id,
                CategoryId = trainingViewModel.CategoryId,
                TrainingName = trainingViewModel.TrainingName,
                TrainingDesc = trainingViewModel.TrainingDesc,
                TrainingAuthor = trainingViewModel.TrainingAuthor,
                CreatedBy = "Static Admin",
                CreatedTime = DateTime.Now,
                UpdatedBy = "Static Admin",
                UpdatedTime = DateTime.Now,
            };

            _trainingRepository.AddTraining(training);
        }

        public List<Training> GetTraining()
        {
            var training = _trainingRepository.GetTraining();
            return training;
        }
        /*
        public Category GetCategory(int id)
        {
            var category = _categoryRepository.GetCategory(id);

            return category;
        }

        public bool UpdateCategory(CategoryViewModel categoryViewModel)
        {
            Category category = _categoryRepository.GetCategory(categoryViewModel.Id);
            if (category != null)
            {
                category.Id = categoryViewModel.Id;
                category.CategoryName = categoryViewModel.CategoryName;
                category.CategoryDesc = categoryViewModel.CategoryDesc;
                category.UpdatedBy = "Static Admin";
                category.UpdatedTime = System.DateTime.Now;

                _categoryRepository.UpdateCategory(category);
                return true;
            }

            return false;
        }*/
        public bool DeleteTraining(TrainingViewModel trainingViewModel)
        {
            Training training = _trainingRepository.GetTraining(trainingViewModel.Id);
            if (training != null)
            {
                _trainingRepository.DeleteTraining(training);
                return true;
            }

            return false;
        }
    }
}
