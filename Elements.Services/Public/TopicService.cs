namespace Elements.Services.Public
{
    using AutoMapper;
    using Elements.Data;
    using Elements.Models.Forum;
    using Elements.Services.Models.Forum.ViewModels;
    using Elements.Services.Public.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TopicService : BaseEFService, ITopicService
    {
        public TopicService(ElementsContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public async Task<Topic> AddReplyAsync(int topicId, Reply newReply)
        {
            var topic = this.Context.Topics.FirstOrDefault(t => t.Id == topicId);

            if (topic != null)
            {
                newReply.CreateDate = DateTime.Now;
                newReply.IsActive = true;

                topic.Replies.Add(newReply);

                await this.Context.SaveChangesAsync();
            }

            return topic;
        }

        public async Task<Topic> AddTopicAsync(Topic topic)
        {
            topic.CreateDate = DateTime.Now;

            if (topic.TopicType == 0)
            {
                topic.TopicType = TopicType.Common;
            }

            this.Context.Topics.Add(topic);
            await this.Context.SaveChangesAsync();

            return topic;
        }

        public IEnumerable<TopicOfCategoryViewModel> GetAllTopicsOfCategory(int categoryId, bool? activeOnly)
        {
            var topicsOfCategoryViewModel = this.Context.Topics
              .Include(t => t.Author)
              .Include(t => t.Replies)
              .Where(t => t.CategoryId == categoryId)
              .Select(c => new TopicOfCategoryViewModel()
              {
                  Title = c.Title,
                  TopicId = c.Id,
                  AuthorName = c.Author.UserName,
                  AuthorId = c.Author.Id,
                  CreateDate = c.CreateDate,
                  NumberOfReply = c.Replies.Count,
                  TopicType = c.TopicType,
              })
              .OrderByDescending(t => t.TopicType)
              .ThenByDescending(t => t.CreateDate);

            if (activeOnly.HasValue)
            {
                // TODO: add is active to topics
                // topicsOfCategoryViewModel = topicsOfCategoryViewModel.Where(t => t.IsActive == activeOnly);
            }

            return topicsOfCategoryViewModel;
        }

        public Topic GetTopicWithReplies(int topicId)
        {
            var topicWithReplies = this.Context.Topics
                                                  .Include(t => t.Author)
                                                  .Include(t => t.Replies)
                                                  .ThenInclude((Reply r) => r.Author)
                                                  .Where(t => t.IsActive)
                                                  .FirstOrDefault(t => t.Id == topicId);

            return topicWithReplies;
        }
    }
}
