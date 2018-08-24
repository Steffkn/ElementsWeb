namespace Elements.Services.Models.Areas.Admin.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int NumberOfTopics { get; set; }

        public bool IsActive { get; set; }
    }
}
