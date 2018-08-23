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
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class TopicService : BaseEFService, ITopicService
    {
        private readonly IDateTimeService dateTimeService;

        public TopicService(ElementsContext context, IMapper mapper, IDateTimeService dateTimeService)
            : base(context, mapper)
        {
            this.dateTimeService = dateTimeService;
        }

        public async Task<Topic> AddReplyAsync(Reply newReply)
        {
            var topic = this.Context.Topics.FirstOrDefault(t => t.Id == newReply.TopicId);
            if (topic != null)
            {
                newReply.CreateDate = dateTimeService.Now;
                newReply.IsActive = true;
                newReply.TopicId = topic.Id;

                await this.Context.Replies.AddAsync(newReply);
                await this.Context.SaveChangesAsync();
            }

            return topic;
        }

        public async Task<Topic> AddTopicAsync(Topic topic)
        {
            topic.CreateDate = dateTimeService.Now;

            if (topic.TopicType == 0)
            {
                topic.TopicType = TopicType.Common;
            }

            await this.Context.Topics.AddAsync(topic);
            await this.Context.SaveChangesAsync();

            return topic;
        }

        public IEnumerable<TopicOfCategoryViewModel> GetAllTopicsOfCategory(int categoryId, bool? activeOnly)
        {
            if (activeOnly.HasValue)
            {
                return this.Context.Topics
                  .Include(t => t.Author)
                  .Include(t => t.Replies)
                  .Where(t => t.CategoryId == categoryId && t.IsActive == activeOnly)
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
            }
            else
            {
                return this.Context.Topics
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
            }
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

        public IEnumerable<TopicOfCategoryViewModel> Where(Expression<Func<Topic, bool>> searchTerms)
        {
            return this.Context.Topics
                .Where(searchTerms)
                .Select(c => new TopicOfCategoryViewModel()
                {
                    Title = c.Title,
                    TopicId = c.Id,
                    AuthorName = c.Author.UserName,
                    AuthorId = c.Author.Id,
                    CreateDate = c.CreateDate,
                    NumberOfReply = c.Replies.Count,
                    TopicType = c.TopicType,
                    ImageUrl = c.ImageUrl
                })
                .OrderBy(t => t.CreateDate);
        }
    }
}
