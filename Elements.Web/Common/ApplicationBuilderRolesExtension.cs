using Elements.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elements.Web.Common
{
    public static class ApplicationBuilderRolesExtension
    {
        private static string DefaultAdminUsername = "admin";
        private static string DefaultAdminPassword = "admin123";

        private static readonly IdentityRole[] roles =
        {
            new IdentityRole("Administrator"),
            new IdentityRole("Moderator"),
            new IdentityRole("User")
        };

        public static async void SeedDatabase(this IApplicationBuilder app)
        {
            var serviceScoreFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var scope = serviceScoreFactory.CreateScope();
            using (scope)
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name))
                    {
                        var reuslt = await roleManager.CreateAsync(role);
                    }
                }

                var user = await userManager.FindByNameAsync(DefaultAdminUsername);

                if (user == null)
                {
                    user = new User()
                    {
                        UserName = DefaultAdminUsername,
                        Email = DefaultAdminUsername + "@admins.com"
                    };

                    var result = await userManager.CreateAsync(user, DefaultAdminPassword);
                    result = await userManager.AddToRoleAsync(user, roles[0].Name);
                }
            }
        }
    }
}
