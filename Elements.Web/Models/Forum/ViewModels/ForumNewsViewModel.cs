namespace Elements.Web.Models.Forum.ViewModels
{
    using System.Collections.Generic;

    public class ForumNewsViewModel
    {
        public ForumNewsViewModel()
        {
            this.TopicsOverview = new HashSet<TopicOverviewViewModel>();
        }

        public ICollection<TopicOverviewViewModel> TopicsOverview { get; set; }
    }
}
