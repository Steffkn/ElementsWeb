using System.Collections.Generic;
using Elements.Services.Models.Forum.ViewModels;
using Elements.Services.Public.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Elements.Web.Pages
{
    public class NewsModel : PageModel
    {
        private readonly INewsService newsService;

        public NewsModel(INewsService newsService)
        {
            this.newsService = newsService;
        }

        public IEnumerable<TopicOverviewViewModel> Topics { get; set; }

        public void OnGet()
        {
            int count = 5;
            this.Topics = this.newsService.GetNewsDevTopicsWithAuthors(count);
        }
    }
}