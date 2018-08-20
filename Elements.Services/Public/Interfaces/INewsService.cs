using System.Collections.Generic;
using Elements.Services.Models.Forum.ViewModels;

namespace Elements.Services.Public.Interfaces
{
    public interface INewsService
    {
        IEnumerable<TopicOverviewViewModel> GetNewsDevTopicsWithAuthors(int count);
    }
}
