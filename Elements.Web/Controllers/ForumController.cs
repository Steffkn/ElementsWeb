using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Elements.Data;
using Elements.Models.Forum;
using Elements.Services.Models.Forum.BindingModels;
using Elements.Services.Models.Forum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Elements.Web.Controllers
{
    public class ForumController : Controller
    {
        public ElementsContext Context { get; }

        public ForumController(ElementsContext context)
        {
            this.Context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var categories = this.Context.ForumCategories.Select(c => new ForumCategoryViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IconURL = "images/icons/under-construction-hat.png"
            });

            return View(model: categories);
        }

        [HttpGet]
        public IActionResult News()
        {
            var topics = this.Context.Topics
                            .Include(t => t.Author)
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

        [HttpGet]
        [Authorize]
        public IActionResult AddReply()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddReply(int topicId)
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddTopic()
        {
            var categories = this.Context.ForumCategories.Select(c => new SelectCategoryViewModel() { Name = c.Name, Id = c.Id.ToString() }).ToList();
            var viewModel = new AddTopicViewModel()
            {
                Categories = categories
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
        public IActionResult ShowTopic(int topicId)
        {
            var topicsOfCategoryViewModel = this.Context.Topics
                .Include(t => t.Author)
                .Include(t => t.Replies)
                .Where(t => t.CategoryId == topicId)
                .Select(c => new TopicOfCategoryViewModel()
                {
                    Title = c.Title,
                    TopicId = c.Id,
                    AuthorName = c.Author.UserName,
                    AuthorId = c.Author.Id,
                    CreateDate = c.CreateDate,
                    NumberOfReply = c.Replies.Count,
                    TopicType = c.TopicType,
                    ImageUrl = GetImage(c.TopicType)
                });

            return View(topicsOfCategoryViewModel.ToList());
        }

        private string GetImage(TopicType topicType)
        {
            string result = string.Empty;

            switch (topicType)
            {
                case TopicType.Common:
                    result = "fas fa-sticky-note fa-fw";
                    break;
                case TopicType.Info:
                    result = "fas fa-info-circle fa-fw";
                    break;
                case TopicType.Important:
                    result = "fas fa-exclamation-triangle fa-fw";
                    break;
                case TopicType.News:
                    result = "fas fa-sticky-note fa-fw";
                    break;
                case TopicType.Development:
                    result = "fas fa-code fa-fw";
                    break;
                default:
                    result = "fas fa-sticky-note fa-fw";
                    break;
            }

            return result;
        }

        [HttpGet]
        public IActionResult Topic(int topicId)
        {
            var topic = this.Context.Topics
                .Include(t => t.Replies)
                .Include(t => t.Author)
                .FirstOrDefault(t => t.Id == topicId);

            if (topic == null)
            {
                return this.RedirectToAction("Index");
            }

            return View(topic);
        }
    }
}