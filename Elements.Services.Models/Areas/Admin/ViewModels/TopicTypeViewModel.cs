using System;
using System.Collections.Generic;
using System.Text;

namespace Elements.Services.Models.Areas.Admin.ViewModels
{
    public class TopicTypeViewModel
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
