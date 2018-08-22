using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Elements.Common;
using Elements.Models.Forum;
using Elements.Services.Admin.Interfaces;
using Elements.Services.Models.Forum.ViewModels;
using Elements.Services.Public.Interfaces;
using Elements.Web.Common;
using Elements.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Elements.Web.Areas.Admin.Pages
{
    [Authorize(Policy = Constants.PolicyRequireAdminDevRole)]
    public class AddNewsModel : PageModel
    {
        // ~500kb
        private const int DefaultFileSize = 500000;

        private readonly IManageNewsService newsService;
        private readonly IDateTimeService dateTimeService;

        public AddNewsModel(
            IManageNewsService newsService,
            IDateTimeService dateTimeService)
        {
            this.newsService = newsService;
            this.dateTimeService = dateTimeService;
            this.TopicTypes = new HashSet<TopicTypeViewModel>();
        }

        [BindProperty]
        [Required]
        public string Title { get; set; }

        [BindProperty]
        [Required]
        public string TopicContent { get; set; }

        [BindProperty]
        [Required]
        public int TopicTypeId { get; set; }

        public HashSet<TopicTypeViewModel> TopicTypes { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public void OnGet()
        {
            this.AddTopicType(TopicType.News);
            this.AddTopicType(TopicType.Development);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (CustomValidator.IsFormFileLenghtBiggerThan(this.ImageFile, DefaultFileSize))
            {
                this.ModelState.AddModelError(string.Empty, "Please select smaller image (jpeg or png)");
            }

            if (!CustomValidator.IsFormFileInFormat(this.ImageFile, "image/png", "image/jpeg", "image/jpg"))
            {
                this.ModelState.AddModelError(string.Empty, "Please select a valid image file (jpeg or png)");
            }

            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var fileName = ImageManager.GetNewFileName(this.ImageFile.FileName);
            var imageUrl = ImageManager.GetIconRelativePath("news", fileName);

            // TODO: extract this and the path
            var topic = new Topic()
            {
                AuthorId = this.User.GetUserId(),
                CategoryId = (int)ForumCategoryType.News,
                Content = this.TopicContent,
                Title = this.Title,
                CreateDate = dateTimeService.Now,
                TopicType = (TopicType)TopicTypeId,
                ImageUrl = imageUrl
            };

            this.newsService.AddNews(topic);

            var fullFilePathName = ImageManager.GetFullFilePath("news", fileName);

            using (var fileStream = new FileStream(fullFilePathName, FileMode.Create))
            {
                await this.ImageFile.CopyToAsync(fileStream);
            }

            return this.RedirectToPage("MainPanel");
        }

        private void AddTopicType(TopicType topicType)
        {
            var topicTypeVeiwModel = new TopicTypeViewModel()
            {
                Id = (int)topicType,
                Value = topicType.ToString()
            };

            if (!this.TopicTypes.Contains(topicTypeVeiwModel))
            {
                this.TopicTypes.Add(topicTypeVeiwModel);
            }
        }
    }
}