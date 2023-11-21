﻿using ASI.Basecode.AdminApp.Mvc;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;

namespace ASI.Basecode.AdminApp.Controllers
{
    public class TopicController : ControllerBase<TopicController>
    {
        private readonly ITopicService _topicService;
        private readonly ITrainingService _trainingService;

        public TopicController(ITopicService topicService,
                               ITrainingService trainingService,
                               IHttpContextAccessor httpContextAccessor,
                               ILoggerFactory loggerFactory,
                               IConfiguration configuration,
                               IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _topicService = topicService;
            _trainingService = trainingService;
        }
        //TrainingId is passed through ViewBag
        public IActionResult Topics(int trainingId)
        {
            ViewBag.TrainingId = trainingId;

            var training = _trainingService.GetTraining(trainingId);
            if (training != null)
            {
                ViewBag.TrainingName = training.TrainingName;
            }

            var topics = _topicService.GetTopicsByTrainingId(trainingId);
            return View(topics);
        }

        public IActionResult CreateTopic(int trainingId)
        {
            ViewBag.TrainingId = trainingId; // Set ViewBag.TrainingId to the provided trainingId
            return View();
        }

        [HttpPost]
        public IActionResult CreateTopic(TopicViewModel topicViewModel, int trainingId)
        {
            _topicService.AddTopic(topicViewModel, this.UserId, trainingId);
            return RedirectToAction("Topics", new { trainingId });
        }

        [HttpGet]
        public IActionResult ViewTopic(int id, int trainingId)
        {
            ViewBag.TrainingId = trainingId;
            var topic = _topicService.GetTopic(id, trainingId);
            if (topic != null)
            {
                TopicViewModel topicViewModel = new()
                {
                    TopicId = id,
                    TopicName = topic.TopicName,
                    TopicDesc = topic.TopicDesc,
                };
                return View(topicViewModel);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult EditTopic(int id, int trainingId)
        {
            ViewBag.TrainingId = trainingId;
            var topic = _topicService.GetTopic(id, trainingId);
            if (topic != null)
            {
                //List<CategoryViewModel> categoryViewModels = GetCategoryViewModels();

                //int selectedCategoryId = training.CategoryId;
                var url = "https://127.0.0.1:8080/";

                TopicViewModel topicViewModel = new()
                {
                    TopicId = id,
                    TopicName = topic.TopicName,
                    TopicDesc = topic.TopicDesc,
                    FileUrl = Path.Combine(url, topic.TopicFile + ".png"),
                };

                return View(topicViewModel);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult EditTopic(TopicViewModel topicViewModel, int trainingId)
        {
            bool isUpdated = _topicService.UpdateTopic(topicViewModel, this.UserName, trainingId);
            if (isUpdated)
            {
                return RedirectToAction("Topics", new { trainingId });
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult DeleteTopic(int id, int trainingId)
        {
            bool isDeleted = _topicService.DeleteTopic(id, trainingId);
            if (isDeleted)
            {
                return RedirectToAction("Topics", new { trainingId });
            }
            return NotFound();
        }

    }
}
