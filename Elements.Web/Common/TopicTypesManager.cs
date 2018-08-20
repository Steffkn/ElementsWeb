using Elements.Models.Forum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Elements.Web.Common
{
    public class TopicTypesManager
    {
        public static HashSet<TopicType> GetTopicTypes(params TopicType[] topicTypes)
        {
            var topics = new HashSet<TopicType>(topicTypes.Distinct());

            return topics;
        }

        public static HashSet<TopicType> GetAllExcept(params TopicType[] topicTypes)
        {
            var topics = new HashSet<TopicType>();

            // add all
            foreach (TopicType item in Enum.GetValues(typeof(TopicType)))
            {
                topics.Add(item);
            }

            foreach (TopicType item in topicTypes.Distinct())
            {
                topics.Remove(item);
            }

            return topics;
        }

        public static string GetImage(TopicType topicType)
        {
            string result = string.Empty;

            switch (topicType)
            {
                case TopicType.Common:
                    result = "fas fa-sticky-note fa-fw";
                    break;
                case TopicType.Info:
                    result = "fas fa-info-circle fa-fw";
                    break;
                case TopicType.Important:
                    result = "fas fa-exclamation-triangle fa-fw";
                    break;
                case TopicType.News:
                    result = "fas fa-sticky-note fa-fw";
                    break;
                case TopicType.Development:
                    result = "fas fa-code fa-fw";
                    break;
                default:
                    result = "fas fa-sticky-note fa-fw";
                    break;
            }

            return result;
        }

    }
}
