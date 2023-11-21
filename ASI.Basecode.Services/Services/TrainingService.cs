using ASI.Basecode.Data;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly KnowBody_DBContext _dbContext;
        private readonly ITrainingRepository _trainingRepository;

        public TrainingService(ITrainingRepository trainingRepository, KnowBody_DBContext dbContext)
        {
            _trainingRepository = trainingRepository;
            _dbContext = dbContext;
        }

        public void AddTraining(TrainingViewModel trainingViewModel, string username)
        {
            if (!_trainingRepository.TrainingExists(trainingViewModel.TrainingName))
            {
                var coverImagesPath = PathManager.DirectoryPath.CoverImagesDirectory;
                Training training = new Training
                {
                    Id = trainingViewModel.Id,
                    CategoryId = trainingViewModel.CategoryId,
                    TrainingName = trainingViewModel.TrainingName,
                    TrainingDesc = trainingViewModel.TrainingDesc,
                    TrainingAuthor = trainingViewModel.TrainingAuthor,
                    TrainingImage = Guid.NewGuid().ToString(),
                    CreatedBy = username,
                    CreatedTime = DateTime.Now,
                    UpdatedBy = username,
                    UpdatedTime = DateTime.Now
                };

                var coverImageFileName = Path.Combine(coverImagesPath, training.TrainingImage) + ".png";
                using (var fileStream = new FileStream(coverImageFileName, FileMode.Create))
                {
                    trainingViewModel.ImageFile.CopyTo(fileStream);
                }

                _trainingRepository.AddTraining(training);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.TrainingExists);
            }
        }

        public List<Training> GetTraining()
        {
            var training = _trainingRepository.GetTraining();
            return training;
        }

        public Training GetTraining(int id)
        {
            var training = _trainingRepository.GetTraining(id);

            return training;
        }

        
        public bool UpdateTraining(TrainingViewModel trainingViewModel, string username)
        {
            Training training = _trainingRepository.GetTraining(trainingViewModel.Id);
            if (training != null)
            {
                training.Id = trainingViewModel.Id;
                training.CategoryId = trainingViewModel.CategoryId;
                training.TrainingName = trainingViewModel.TrainingName;
                training.TrainingDesc = trainingViewModel.TrainingDesc;
                training.TrainingAuthor = trainingViewModel.TrainingAuthor;
                training.UpdatedBy = username;
                training.UpdatedTime = System.DateTime.Now;

                _trainingRepository.UpdateTraining(training);
                return true;
            }

            return false;
        }
        /*
        public void UpdateTraining(TrainingViewModel updatedTraining, string username)
        {
            var existingTraining = _trainingRepository.GetTraining(updatedTraining.Id);

            if (existingTraining != null)
            {
                // Update the fields that can change
                existingTraining.CategoryId = updatedTraining.CategoryId;
                existingTraining.TrainingName = updatedTraining.TrainingName;
                existingTraining.TrainingDesc = updatedTraining.TrainingDesc;
                existingTraining.TrainingAuthor = updatedTraining.TrainingAuthor;
                existingTraining.UpdatedBy = username;
                existingTraining.UpdatedTime = DateTime.Now;

                // Check if a new image is provided
                if (updatedTraining.ImageFile != null)
                {
                    var coverImagesPath = PathManager.DirectoryPath.CoverImagesDirectory;

                    // Delete the old image file
                    var oldImagePath = Path.Combine(coverImagesPath, existingTraining.TrainingImage) + ".png";
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }

                    // Generate a new GUID for the image file name
                    existingTraining.TrainingImage = Guid.NewGuid().ToString();

                    // Save the new image file
                    var newImagePath = Path.Combine(coverImagesPath, existingTraining.TrainingImage) + ".png";
                    using (var fileStream = new FileStream(newImagePath, FileMode.Create))
                    {
                        updatedTraining.ImageFile.CopyTo(fileStream);
                    }
                }

                // Update the training in the repository
                _trainingRepository.UpdateTraining(existingTraining);
            }
           
        }*/



        public List<Training> GetTrainingsByCategoryId(int categoryId)
        {
            return _dbContext.Trainings
                .Where(t => t.CategoryId == categoryId)
                .ToList();
        }

        public List<TrainingViewModel> GetTrainings()
        {
            var url = "https://127.0.0.1:8080/";
            var data = _trainingRepository.GetTrainings().Select(s => new TrainingViewModel
            {
                TrainingName = s.TrainingName,
                TrainingAuthor = s.TrainingAuthor,
                ImageUrl = Path.Combine(url, s.TrainingImage + ".png"),
        }).ToList();
            return data;
        }

        public TrainingViewModel GetTrainingViewModel(Training training, int id, Category category)
        {
            var model = new TrainingViewModel();
            var url = "https://127.0.0.1:8080/";

            model = new TrainingViewModel
            {
                Id = id,
                TrainingName = training.TrainingName,
                TrainingDesc = training.TrainingDesc,
                TrainingAuthor = training.TrainingAuthor,
                CategoryId = training.CategoryId,
                CategoryName = category != null ? category.CategoryName : "No category selected",
                ImageUrl = Path.Combine(url, training.TrainingImage + ".png"),
            };
            return model;
        }

        public TrainingViewModel GetEditTrainingViewModel(Training training, int id, Category category, List<CategoryViewModel> categoryViewModels)
        {
            var model = new TrainingViewModel();
            var url = "https://127.0.0.1:8080/";

            model = new TrainingViewModel
            {
                Id = id,
                TrainingName = training.TrainingName,
                TrainingDesc = training.TrainingDesc,
                TrainingAuthor = training.TrainingAuthor,
                CategoryId = training.CategoryId,
                Categories = categoryViewModels, // Pass category view models received from the controller
                CategoryName = category != null ? category.CategoryName : "No category selected",
                ImageUrl = Path.Combine(url, training.TrainingImage + ".png"),
            };

            

            return model;
        }

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
