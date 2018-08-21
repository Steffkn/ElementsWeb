namespace Elements.Web.Common
{
    using Elements.Models.Forum;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CategoryTypesManager
    {
        public static HashSet<ForumCategoryType> GetCategoryTypes(params ForumCategoryType[] categoryTypes)
        {
            var categories = new HashSet<ForumCategoryType>(categoryTypes.Distinct());

            return categories;
        }

        public static HashSet<ForumCategoryType> GetAll()
        {
            var categories = new HashSet<ForumCategoryType>();

            // add all
            foreach (ForumCategoryType item in Enum.GetValues(typeof(ForumCategoryType)))
            {
                categories.Add(item);
            }

            return categories;
        }

        public static HashSet<ForumCategoryType> GetAllExcept(params ForumCategoryType[] topicTypes)
        {
            var categories = new HashSet<ForumCategoryType>();

            // add all
            foreach (ForumCategoryType item in Enum.GetValues(typeof(ForumCategoryType)))
            {
                categories.Add(item);
            }

            foreach (ForumCategoryType item in topicTypes.Distinct())
            {
                categories.Remove(item);
            }

            return categories;
        }
    }
}
