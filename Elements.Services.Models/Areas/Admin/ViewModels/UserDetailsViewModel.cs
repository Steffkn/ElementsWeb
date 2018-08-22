using Elements.Models.Forum;
using System;
using System.Collections.Generic;

namespace Elements.Services.Models.Areas.Admin.ViewModels
{
    public class UserDetailsViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Avatar { get; set; }

        public bool IsRestricted { get; set; }

        public DateTime RestrictionEndDate { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<Topic> Topics { get; set; }

        public IEnumerable<Reply> Replies { get; set; }
    }
}
