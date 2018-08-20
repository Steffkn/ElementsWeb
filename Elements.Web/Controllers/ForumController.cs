namespace Elements.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Elements.Data;
    using Elements.Models.Forum;
    using Elements.Services.Models.Forum.BindingModels;
    using Elements.Services.Models.Forum.ViewModels;
    using Elements.Web.Common;
    using Elements.Web.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class ForumController : Controller
    {
        private readonly IMapper mapper;

        public ElementsContext Context { get; }

        public ForumController(ElementsContext context, IMapper mapper)
        {
            this.Context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var categories = this.Context.ForumCategories.Select(c => new ForumCategoryViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IconURL = "/images/icons/under-construction-hat.png",
                CategoryType = c.CategoryType
            });

            Dictionary<ForumCategoryType, List<ForumCategoryViewModel>> categoriesss = new Dictionary<ForumCategoryType, List<ForumCategoryViewModel>>();

            foreach (var item in categories)
            {
                if (!categoriesss.ContainsKey(item.CategoryType))
                {
                    categoriesss.Add(item.CategoryType, new List<ForumCategoryViewModel>());
                }

                categoriesss[item.CategoryType].Add(item);
            }

            return View(model: categoriesss);
        }

        [HttpGet]
        public IActionResult News()
        {
            var topics = this.Context.Topics
                            .Include(t => t.Author)
                            .Where(t => t.TopicType == TopicType.News || t.TopicType == TopicType.Development)
                            .Select(t => new TopicOverviewViewModel()
                            {
                                AuthorId = t.Author.Id,
                                AuthorName = t.Author.UserName,
                                Content = t.Content,
                                Id = t.Id,
                                CreateDate = t.CreateDate,
                                Title = t.Title,
                                ImageUrl = t.ImageUrl
                            })
                            .OrderBy(m => m.CreateDate)
                            .ToList();
            return View(topics);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddReply(int topicId, string replyContent)
        {
            var topic = this.Context.Topics.FirstOrDefault(t => t.Id == topicId);

            if (topic != null)
            {
                topic.Replies.Add(new Reply() { AuthorId = this.User.GetUserId(), Content = replyContent, CreateDate = DateTime.Now, IsActive = true });

                this.Context.SaveChanges();
            }

            return this.RedirectToAction("ShowTopic", new { topicId = topicId });
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddTopic()
        {
            var categories = this.Context.ForumCategories.Select(c => new SelectCategoryViewModel() { Name = c.Name, Id = c.Id.ToString() }).ToList();
            var availableTopicTypes = TopicTypesManager.GetAllExcept(TopicType.News, TopicType.Development);
            var viewModel = new AddTopicViewModel()
            {
                Categories = categories,
                TopicTypes = availableTopicTypes.Select(x => new TopicTypeViewModel() { Id = (int)x, Name = x.ToString() })
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddTopic(AddTopicBindingModel newTopic)
        {
            var imageFile = newTopic.ImageUrl;

            if (imageFile != null)
            {
                if (imageFile.ContentType != "image/png" && imageFile.ContentType != "image/jpeg")
                {
                    this.ModelState.AddModelError("", "Please select a valid image file (jpeg or png)");
                }

                if (imageFile.Length > 500000)
                {
                    this.ModelState.AddModelError("", "Please select smaller image (jpeg or png)");
                }
            }

            if (!this.ModelState.IsValid)
            {
                var categories = this.Context.ForumCategories.Select(c => new SelectCategoryViewModel() { Name = c.Name, Id = c.Id.ToString() }).ToList();
                var viewModel = new AddTopicViewModel()
                {
                    Categories = categories
                };

                return this.View(viewModel);
            }

            var user = this.Context.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

            if (user != null)
            {
                // TODO: extract this and the path
                var topic = new Topic()
                {
                    AuthorId = user.Id,
                    CategoryId = newTopic.CategoryId,
                    Content = newTopic.Content,
                    Title = newTopic.Title,
                    CreateDate = DateTime.Now,
                    TopicType = newTopic.TopicType,
                };

                if (imageFile != null)
                {
                    var fileName = Guid.NewGuid().ToString() + imageFile.FileName + ".jpg";
                    var fullFilePathName = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "news", fileName);
                    var imageUrl = "/images/news/" + fileName;

                    topic.ImageUrl = imageUrl;

                    var fileStream = new FileStream(fullFilePathName, FileMode.Create);
                    await newTopic.ImageUrl.CopyToAsync(fileStream);
                }

                this.Context.Topics.Add(topic);
                await this.Context.SaveChangesAsync();

                return this.RedirectToAction("Index");
            }

            return this.View(newTopic);
        }

        [HttpGet]
        public IActionResult Category(int id)
        {
            var topicsOfCategoryViewModel = this.Context.Topics
                .Include(t => t.Author)
                .Include(t => t.Replies)
                .Where(t => t.CategoryId == id)
                .Select(c => new TopicOfCategoryViewModel()
                {
                    Title = c.Title,
                    TopicId = c.Id,
                    AuthorName = c.Author.UserName,
                    AuthorId = c.Author.Id,
                    CreateDate = c.CreateDate,
                    NumberOfReply = c.Replies.Count,
                    TopicType = c.TopicType,
                    ImageUrl = TopicTypesManager.GetImage(c.TopicType)
                })
                .OrderByDescending(t => t.TopicType)
                .ThenByDescending(t => t.CreateDate);

            return View(topicsOfCategoryViewModel.ToList());
        }

        [HttpGet]
        public IActionResult Topic(int id)
        {
            var topicWithReplies = this.Context.Topics
                .Include(t => t.Author)
                .Include(t => t.Replies)
                .ThenInclude((Reply r) => r.Author)
                .Where(t => t.IsActive)
                .FirstOrDefault(t => t.Id == id);

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

            return View(topicsWithRepliesViewModel);
        }
    }
}