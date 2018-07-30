using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elements.Data;
using Elements.Models.Forum;
using Elements.Web.Models.Forum.BindingModels;
using Elements.Web.Models.Forum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            return View();
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
                            .ToList();
            return View(topics);
        }

        [HttpGet]
        public IActionResult Topic(int topicId)
        {
            return View();
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
            var categories = this.Context.ForumCategories.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();
            var viewModel = new TopicViewModel()
            {
                Categories = categories
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddTopic(TopicBindingModels newTopic)
        {
            if (!this.ModelState.IsValid)
            {
                var categories = this.Context.ForumCategories.Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() }).ToList();
                var viewModel = new TopicViewModel()
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
    }
}