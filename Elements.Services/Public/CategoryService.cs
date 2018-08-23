using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Elements.Data;
using Elements.Models.Forum;
using Elements.Services.Models.Forum.ViewModels;
using Elements.Services.Public.Interfaces;

namespace Elements.Services.Public
{
    public class CategoryService : BaseEFService, ICategoryService
    {
        public CategoryService(ElementsContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public IEnumerable<ForumCategoryViewModel> GetAllCategories(bool? active = null)
        {
            var categoriesToGet = active.HasValue ? this.Context.ForumCategories.Where(c => c.IsActive == active) : this.Context.ForumCategories;

            IEnumerable<ForumCategoryViewModel> categories =
                this.Mapper.Map<IEnumerable<ForumCategory>, IEnumerable<ForumCategoryViewModel>>(categoriesToGet);

            return categories;
        }

        public IEnumerable<SelectCategoryViewModel> GetAllCategoriesForSelect(bool? active = null)
        {
            var categoriesToGet = active.HasValue ? this.Context.ForumCategories.Where(c => c.IsActive == active) : this.Context.ForumCategories;

            var categories = this.Mapper.Map<IEnumerable<ForumCategory>, IEnumerable<SelectCategoryViewModel>>(categoriesToGet);
            return categories;
        }

        public Dictionary<ForumCategoryType, List<ForumCategoryViewModel>> GetAllActiveCategoriesInGroups()
        {
            var categories = GetAllCategories(active: true);
            var groupedCategories = new Dictionary<ForumCategoryType, List<ForumCategoryViewModel>>();
            foreach (var item in categories)
            {
                if (!groupedCategories.ContainsKey(item.CategoryType))
                {
                    groupedCategories.Add(item.CategoryType, new List<ForumCategoryViewModel>());
                }

                groupedCategories[item.CategoryType].Add(item);
            }

            return groupedCategories;
        }
    }
}
