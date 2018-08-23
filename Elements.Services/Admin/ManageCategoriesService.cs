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

        public async Task<bool> AddAsync(ForumCategory model)
        {
            if (model == null)
            {
                return false;
            }

            await this.Context.ForumCategories.AddAsync(model);
            await this.Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = this.Context.ForumCategories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return false;
            }

            category.IsActive = false;
            await this.Context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditCategoryAsync(EditCategoryBindingModel model)
        {
            if (model == null)
            {
                return false;
            }

            var category = this.Context.ForumCategories.FirstOrDefault(c => c.Id == model.Id);

            if (category == null)
            {
                return false;
            }

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

        public IEnumerable<T> GetCategories<T>()
            where T : class
        {
            var categories = this.Context.ForumCategories.Include(c => c.Topics);
            var categoriesViewModel = this.Mapper.Map<IEnumerable<T>>(categories);
            return categoriesViewModel;
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
