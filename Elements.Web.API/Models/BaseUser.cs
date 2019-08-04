using System;

namespace Elements.Web.API.Models
{
    public class BaseUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
