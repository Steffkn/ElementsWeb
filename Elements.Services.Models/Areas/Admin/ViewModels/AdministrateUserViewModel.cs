namespace Elements.Services.Models.Areas.Admin.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class AdministrateUserViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string NormalizedEmail { get; set; }

        public bool EmailConfirmed { get; set; }

        public DateTime RegisterDate { get; set; }

        public string Role { get; set; }

        public bool IsActive { get; set; }
    }
}
