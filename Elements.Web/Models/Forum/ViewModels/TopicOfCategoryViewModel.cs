using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elements.Web.Models.Forum.ViewModels
{
    public class TopicOfCategoryViewModel
    {
        public int TopicId { get; set; }

        public string Title { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public DateTime CreateDate { get; set; }

        public int NumberOfReply { get; set; }
    }
}
