namespace Elements.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Elements.Models.Forum;
    using Elements.Services.Models.Forum.BindingModels;
    using Elements.Services.Models.Forum.ViewModels;
    using Elements.Services.Public.Interfaces;
    using Elements.Web.Common;
    using Elements.Web.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ForumController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly ITopicService topicService;

        public ForumController(
            ICategoryService categoryService,
            ITopicService topicService)
        {
            this.categoryService = categoryService;
            this.topicService = topicService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Dictionary<ForumCategoryType, List<ForumCategoryViewModel>> categories = this.categoryService.GetAllCategoriesInGroups();
            return View(model: categories);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReply(int topicId, string replyContent)
        {
            var newReply = new Reply()
            {
                AuthorId = this.User.GetUserId(),
                Content = replyContent,
            };

            Topic topic = await this.topicService.AddReplyAsync(topicId, newReply);

            if (topic == null)
            {
                this.ModelState.AddModelError("", "Something went wrong when posting reply!");
                return this.RedirectToAction("Index", "Forum");
            }

            return this.RedirectToAction("Topic", new { id = topic.Id });
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddTopic()
        {
            var categories = this.categoryService.GetAllCategoriesForSelect();
            var availableTopicTypes = TopicTypesManager.GetAllExcept(TopicType.News, TopicType.Development);
            var viewModel = new AddTopicViewModel()
            {
                Categories = categories,
                TopicTypes = availableTopicTypes.Select(x => new TopicTypeViewModel() { Id = (int)x, Value = x.ToString() })
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddTopic(AddTopicBindingModel newTopic)
        {
            if (!this.ModelState.IsValid)
            {
                var categories = this.categoryService.GetAllCategoriesForSelect();
                var availableTopicTypes = TopicTypesManager.GetAllExcept(TopicType.News, TopicType.Development);
                var viewModel = new AddTopicViewModel()
                {
                    Categories = categories,
                    TopicTypes = availableTopicTypes.Select(x => new TopicTypeViewModel() { Id = (int)x, Value = x.ToString() })
                };

                return this.View(viewModel);
            }

            // TODO: extract this and the path
            var topic = new Topic()
            {
                AuthorId = this.User.GetUserId(),
                CategoryId = newTopic.CategoryId,
                Content = newTopic.Content,
                Title = newTopic.Title,
                TopicType = newTopic.TopicType
            };

            Topic resultTopic = await this.topicService.AddTopicAsync(topic);

            if (resultTopic != null)
            {
                return this.RedirectToAction("Topic", new { id = resultTopic.Id });
            }

            return this.View(newTopic);
        }

        [HttpGet]
        public IActionResult Category(int id)
        {
            IEnumerable<TopicOfCategoryViewModel> topics = this.topicService.GetAllTopicsOfCategory(id, true);

            return this.View(topics);
        }

        [HttpGet]
        public IActionResult Topic(int id)
        {
            Topic topicWithReplies = this.topicService.GetTopicWithReplies(id);

            if (topicWithReplies == null)
            {
                return this.RedirectToAction("Index");
            }

            var topicsWithRepliesViewModel = new TopicWithRepliesViewModel();
            if (topicWithReplies != null)
            {
                topicsWithRepliesViewModel = new TopicWithRepliesViewModel()
                {
                    Id = topicWithReplies.Id,
                    TopicTitle = topicWithReplies.Title,
                    TopicContent = topicWithReplies.Content,
                    CreateDate = topicWithReplies.CreateDate,
                    TopicAuthorViewModel = new AuthorViewModel()
                    {
                        Id = topicWithReplies.Author.Id,
                        Username = topicWithReplies.Author.UserName,
                        Avatar = topicWithReplies.Author.Avatar
                    },
                    Replies = topicWithReplies.Replies
                                                .Where(r => r.IsActive)
                                                .Select(r =>
                                                new ReplyViewModel()
                                                {
                                                    Id = r.Id,
                                                    Content = r.Content,
                                                    AuthorViewModel = new AuthorViewModel()
                                                    {
                                                        Id = r.AuthorId,
                                                        Username = r.Author.UserName,
                                                        Avatar = r.Author.Avatar
                                                    },
                                                    CreateDate = r.CreateDate
                                                }),
                };
            }

            return this.View(topicsWithRepliesViewModel);
        }
    }
}