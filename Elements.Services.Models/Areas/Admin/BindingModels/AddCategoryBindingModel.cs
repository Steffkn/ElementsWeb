namespace Elements.Services.Models.Areas.Admin.BindingModels
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    public class AddCategoryBindingModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; }

        [Required]
        public int MainCategoryId { get; set; }
    }
}
