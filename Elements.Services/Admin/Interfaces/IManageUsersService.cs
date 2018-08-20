namespace Elements.Services.Admin.Interfaces
{
    using Elements.Services.Models.Areas.Admin.ViewModels;
    using System.Collections.Generic;

    public interface IManageUsersService
    {
        IEnumerable<AdministrateUserViewModel> GetAllUsersWithTopics();

        IEnumerable<AdministrateUserViewModel> GetUsers();

        AdministrateUserViewModel GetUserWithTopics(string userId);
    }
}
