namespace Elements.Services.Admin.Interfaces
{
    using Elements.Services.Models.Areas.Admin.ViewModels;
    using System.Collections.Generic;

    public interface IAdminUsersService
    {
        IEnumerable<AdministrateUserViewModel> GetAllUsersWithTopics();

        AdministrateUserViewModel GetUserWithTopics(string userId);
    }
}
