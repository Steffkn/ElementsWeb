namespace Elements.Services.Models.Areas.Admin.ViewModels
{
    using System;

    public class AdministrateUserViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string NormalizedEmail { get; set; }

        public bool EmailConfirmed { get; set; }

        public DateTime RegisterDate { get; set; }
    }
}
