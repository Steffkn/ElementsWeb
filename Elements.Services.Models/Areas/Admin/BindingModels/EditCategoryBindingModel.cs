namespace Elements.Services.Models.Areas.Admin.BindingModels
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    public class EditCategoryBindingModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string IconUrl { get; set; }

        public IFormFile ImageFile { get; set; }

        [Required]
        public int MainCategoryId { get; set; }

        [Required]
        public bool IsPrivate { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
