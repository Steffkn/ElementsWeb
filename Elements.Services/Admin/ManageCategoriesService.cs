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
    using Elements.Models.Forum;
    using System.Threading.Tasks;

    public class ManageCategoriesService : BaseEFService, IManageCategoriesService
    {
        public ManageCategoriesService(ElementsContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public bool Add(ForumCategory model)
        {
            this.Context.ForumCategories.Add(model);
            this.Context.SaveChanges();
            return true;
        }

        public bool DeleteCategory(CategoryBindingModel model)
        {
            var category = this.Context.ForumCategories.FirstOrDefault(c => c.Id == model.Id);
            if (category != null)
            {
                this.Context.ForumCategories.Remove(category);
                this.Context.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<bool> EditCategoryAsync(EditCategoryBindingModel model)
        {
            var dbModel = this.Mapper.Map<EditCategoryBindingModel, ForumCategory>(model);
            this.Context.ForumCategories.Update(dbModel);
            await this.Context.SaveChangesAsync();
            return true;
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

            var category = this.Mapper.Map<CategoryViewModel>(categoryFromDb);

            return category;
        }

        public T GetCategoryById<T>(int categoryId)
            where T : class
        {
            var categoryFromDb = this.Context.ForumCategories.Include(c => c.Topics)
                .FirstOrDefault(c => c.Id == categoryId);

            var category = this.Mapper.Map<ForumCategory, T>(categoryFromDb);

            return category;
        }
    }
}
