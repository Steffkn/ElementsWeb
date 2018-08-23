namespace Elements.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Elements.Models;
    using Elements.Models.Forum;
    using Elements.Services.Models.Forum.BindingModels;
    using Elements.Services.Models.Forum.ViewModels;
    using Elements.Services.Public.Interfaces;
    using Elements.Web.Common;
    using Elements.Web.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ForumController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly ITopicService topicService;
        private readonly UserManager<User> userManager;

        public ForumController(
            ICategoryService categoryService,
            ITopicService topicService,
            UserManager<User> userManager)
        {
            this.categoryService = categoryService;
            this.topicService = topicService;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Dictionary<ForumCategoryType, List<ForumCategoryViewModel>> categories = this.categoryService.GetAllActiveCategoriesInGroups();
            return View(model: categories);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReply(int topicId, string replyContent)
        {
            if (replyContent == null)
            {
                return this.View();
            }

            var newReply = new Reply()
            {
                TopicId = topicId,
                AuthorId = this.User.GetUserId(),
                Content = System.Net.WebUtility.HtmlEncode(replyContent),
            };

            Topic topic = await this.topicService.AddReplyAsync(newReply);

            if (topic == null)
            {
                this.ModelState.AddModelError(string.Empty, "Something went wrong when posting reply!");
                return this.RedirectToAction("Index", "Forum");
            }

            return this.RedirectToAction("Topic", new { id = topic.Id });
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddTopic()
        {
            var categories = this.categoryService.GetAllCategoriesForSelect();
            var availableTopicTypes = TopicTypesManager.GetAllExcept(TopicType.News, TopicType.Development, TopicType.Administration);
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
                Content = System.Net.WebUtility.HtmlEncode(newTopic.Content),
                Title = System.Net.WebUtility.HtmlEncode(newTopic.Title),
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
                var authorRoles = this.GetUserRolesById(topicWithReplies.Author.Id);

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
                        Avatar = topicWithReplies.Author.Avatar,
                        Roles = authorRoles
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
                                                        Avatar = r.Author.Avatar,
                                                        Roles = this.GetUserRolesById(r.AuthorId)
                                                    },
                                                    CreateDate = r.CreateDate
                                                }),
                };
            }

            return this.View(topicsWithRepliesViewModel);
        }


        private IEnumerable<string> GetUserRolesById(string userId)
        {
            var user = userManager.FindByIdAsync(userId);
            var roles = userManager.GetRolesAsync(user.Result);

            return roles.Result;
        }
    }
}