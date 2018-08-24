namespace Elements.Services.Public
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Elements.Data;
    using Elements.Models.Forum;
    using Elements.Services.Extensions;
    using Elements.Services.Models.Forum.ViewModels;
    using Elements.Services.Public.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class NewsService : BaseEFService, INewsService
    {
        private const int MaxStringLength = 380;

        public NewsService(ElementsContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public IEnumerable<TopicOverviewViewModel> GetNewsDevTopicsWithAuthors(int count)
        {
            var topics = this.Context.Topics
               .Include(t => t.Author)
               .Where(t => t.TopicType == TopicType.News || t.TopicType == TopicType.Development)
               .Select(t => new TopicOverviewViewModel()
               {
                   AuthorId = t.Author.Id,
                   AuthorName = t.Author.UserName,
                   Content = t.Content.Truncate(MaxStringLength),
                   Id = t.Id,
                   CreateDate = t.CreateDate,
                   Title = t.Title,
                   ImageUrl = t.ImageUrl
               })
               .OrderBy(m => m.CreateDate)
               .Take(count)
               .OrderByDescending(t=>t.CreateDate);

            return topics;
        }
    }
}
