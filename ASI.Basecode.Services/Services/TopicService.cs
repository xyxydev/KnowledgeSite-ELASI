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
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;

        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public void AddTopic(TopicViewModel topicViewModel, string username, int trainingId)
        {

            var coverImagesPath = PathManager.DirectoryPath.CoverImagesDirectory;
            var model = new Topic();
            model.TopicId = topicViewModel.TopicId;
            model.TrainingId = trainingId;
            model.TopicName = topicViewModel.TopicName;
            model.TopicDesc = topicViewModel.TopicDesc;
            model.TopicFile = Guid.NewGuid().ToString();
            model.CreatedBy = username;
            model.CreatedTime = DateTime.Now;
            model.UpdatedBy = username;
            model.UpdatedTime = DateTime.Now;

            var coverImageFileName = Path.Combine(coverImagesPath, model.TopicFile) + ".png";
            using (var fileStream = new FileStream(coverImageFileName, FileMode.Create))
            {
                topicViewModel.ImageFile.CopyTo(fileStream);
            }


            _topicRepository.AddTopic(model);
        }

        public List<Topic> GetTopic()
        {
            var topic = _topicRepository.GetTopic();
            return topic;
        }

        public Topic GetTopic(int topicId, int trainingId)
        {
            var topic = _topicRepository.GetTopic(topicId, trainingId);

            return topic;
        }

        public bool UpdateTopic(TopicViewModel topicViewModel, string username, int trainingId)
        {
            Topic topic = _topicRepository.GetTopic(topicViewModel.TopicId, trainingId);
            if (topic != null)
            {
                //topic.Id = topicViewModel.Id;
                //topic.TrainingId = topicViewModel.TrainingId;
                topic.TopicName = topicViewModel.TopicName;
                topic.TopicDesc = topicViewModel.TopicDesc;
                topic.UpdatedBy = username;
                topic.UpdatedTime = System.DateTime.Now;

                _topicRepository.UpdateTopic(topic);
                return true;
            }

            return false;
        }

        public List<Topic> GetTopicsByTrainingId(int trainingId)
        {
            return _topicRepository.GetTopicsByTrainingId(trainingId);
        }

        public bool DeleteTopic(int id, int trainingId)
        {
            Topic topic = _topicRepository.GetTopic(id, trainingId);
            if (topic != null)
            {
                _topicRepository.DeleteTopic(topic);
                return true;
            }

            return false;
        }

        public bool DeleteTopicsByTrainingId(int trainingId)
        {
            return _topicRepository.DeleteTopicsByTrainingId(trainingId);
        }


    }
}
