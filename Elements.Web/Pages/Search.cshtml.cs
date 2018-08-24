using System.Collections.Generic;
using System.Linq;
using Elements.Services.Models.Forum.ViewModels;
using Elements.Services.Public.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Elements.Web.Pages
{
    public class SearchModel : PageModel
    {
        private readonly ITopicService topicService;

        public SearchModel(ITopicService topicService)
        {
            this.topicService = topicService;
        }

        public IEnumerable<TopicOfCategoryViewModel> Topics { get; set; }

        public bool HasTopics { get; set; }

        public string SearchTerm { get; set; }

        public void OnGet(string searchTerm)
        {
            this.SearchTerm = searchTerm.Trim();
            this.Topics = this.topicService.Where(t => t.Title.Contains(this.SearchTerm));
            this.HasTopics = this.Topics.Any();
        }
    }
}