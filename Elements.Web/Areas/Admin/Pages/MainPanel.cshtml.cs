using System.Collections.Generic;
using System.Linq;
using Elements.Common;
using Elements.Services.Admin.Interfaces;
using Elements.Services.Models.Forum.ViewModels;
using Elements.Services.Public.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Elements.Web.Areas.Admin.Pages
{
    [Authorize(Policy = Constants.PolicyRequireAdminRole)]
    public class MainPanel : PageModel
    {
        private readonly IManageUsersService usersService;
        private readonly ITopicService topicService;

        public MainPanel(IManageUsersService usersService, ITopicService topicService)
        {
            this.usersService = usersService;
            this.topicService = topicService;
        }

        public int TotalUsers { get; set; }

        public bool HasActiveTopics { get; set; }

        public bool HasInActiveTopics { get; set; }

        public IEnumerable<TopicOfCategoryViewModel> ActiveTopics { get; set; }

        public IEnumerable<TopicOfCategoryViewModel> InActiveTopics { get; set; }

        public void OnGet()
        {
            var users = this.usersService.GetUsers();
            this.TotalUsers = users.Count();

            this.ActiveTopics = topicService.Where(t => t.IsActive);
            this.HasActiveTopics = this.ActiveTopics.Any();
            this.InActiveTopics = topicService.Where(t => !t.IsActive);
            this.HasInActiveTopics = this.InActiveTopics.Any();
        }
    }
}