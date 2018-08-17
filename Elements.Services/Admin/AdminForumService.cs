namespace Elements.Services.Admin
{
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

    using Elements.Data;
    using Elements.Services.Admin.Interfaces;
    using Elements.Services.Models.Areas.Admin.ViewModels;
    using AutoMapper;
    using System.Linq;
    using Elements.Services.Models.Areas.Admin.BindingModels;

    public class AdminForumService : BaseEFService, IAdminForumService
    {
        public AdminForumService(ElementsContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public bool DeleteCategory(CategoryBindingModel model)
        {
            var category = this.Context.ForumCategories.Find(model.Id);
            if (category != null)
            {
                this.Context.ForumCategories.Remove(category);
                this.Context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        // TODO: maybe add operation result here? with bool and string
        public bool EditCategory(CategoryBindingModel model)
        {
            var category = this.Context.ForumCategories.Find(model.Id);
            if (category != null)
            {

                category.Name = model.Name;
                category.Description = model.Description;
                this.Context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public IEnumerable<CategoryViewModel> GetCategories()
        {
            var categories = this.Context.ForumCategories.Include(c => c.Topics);
            var categoriesViewModel = this.Mapper.Map<IEnumerable<CategoryViewModel>>(categories);
            return categoriesViewModel;
        }

        public CategoryViewModel GetCategoryById(int categoryId)
        {
            var categoryFromDb = this.Context.ForumCategories.Include(c => c.Topics)
                .FirstOrDefault(c => c.Id == categoryId);

            if (categoryFromDb == null)
            {
                // TODO: do stuff
            }

            var category = this.Mapper.Map<CategoryViewModel>(categoryFromDb);

            return category;
        }
    }
}
