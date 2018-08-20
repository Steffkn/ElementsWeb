using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Elements.Models.Forum;
using Elements.Services.Models.Forum.ViewModels;

namespace Elements.Services.Public.Interfaces
{
    public interface ITopicService
    {
        Task<Topic> AddReplyAsync(int topicId, Reply newReply);
        Task<Topic> AddTopicAsync(Topic topic);
        IEnumerable<TopicOfCategoryViewModel> GetAllTopicsOfCategory(int categoryId, bool? activeOnly);
        Topic GetTopicWithReplies(int topicId);
    }
}
