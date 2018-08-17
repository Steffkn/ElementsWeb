using System;
using System.Linq;
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
                            .Where(t => t.TopicType == TopicType.Common)
                            .Select(t => new TopicOverviewViewModel()
                            {
                                AuthorId = t.Author.Id,
                                AuthorName = t.Author.UserName,
                                Content = t.Content,
                                Id = t.Id,
                                CreateDate = t.CreateDate,
                                Title = t.Title
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
        public IActionResult AddTopic(AddTopicBindingModel newTopic)
        {
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
                var topic = new Topic()
                {
                    AuthorId = user.Id,
                    CategoryId = newTopic.CategoryId,
                    Content = newTopic.Content,
                    Title = newTopic.Title,
                    CreateDate = DateTime.Now,
                    TopicType = TopicType.Common
                };

                this.Context.Topics.Add(topic);
                this.Context.SaveChanges();

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
                    NumberOfReply = c.Replies.Count
                });

            return View(topicsOfCategoryViewModel.ToList());
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