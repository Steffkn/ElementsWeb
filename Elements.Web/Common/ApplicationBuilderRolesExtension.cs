namespace Elements.Web.Common
{
    using Elements.Common;
    using Elements.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class ApplicationBuilderRolesExtension
    {
        private static Tuple<string, string, string> masterUser = new Tuple<string, string, string>("master", "master@admins.com", "secret");
        private static Tuple<string, string, string> adminUser = new Tuple<string, string, string>("admin", "admin@admins.com", "admin123");
        private static Tuple<string, string, string> devUser = new Tuple<string, string, string>("dev", "dev@dev.com", "dev123");
        private static Tuple<string, string, string> modUser = new Tuple<string, string, string>("mod", "mod@admins.com", "mod123");
        private static Tuple<string, string, string> user = new Tuple<string, string, string>("user", "user@example.jm", "123qwe");

        private static IReadOnlyDictionary<string, HashSet<Tuple<string, string, string>>> defaultUsers =
            new Dictionary<string, HashSet<Tuple<string, string, string>>>()
        {
            {
                Constants.CreatorRoleName,
                new HashSet<Tuple<string, string, string>>(){
                    masterUser
                }
            },
        {
                Constants.AdminRoleName,
                new HashSet<Tuple<string, string, string>>(){
                    masterUser,
                    adminUser
                }
            },
        {
                Constants.DevRoleName,
                new HashSet<Tuple<string, string, string>>(){
                    masterUser,
                    devUser
                }
            },
        {
                Constants.ModeratorRoleName,
                new HashSet<Tuple<string, string, string>>(){
                    masterUser,
                    modUser
                }
            },
        {
                Constants.UserRoleName,
                new HashSet<Tuple<string, string, string>>(){
                    masterUser,
                    user
                }
            },
        };

        private static readonly IReadOnlyDictionary<string, HashSet<User>> roles = new Dictionary<string, HashSet<User>>()
        {
            { Constants.CreatorRoleName, new HashSet<User>() },
            { Constants.AdminRoleName, new HashSet<User>() },
            { Constants.DevRoleName, new HashSet<User>() },
            { Constants.ModeratorRoleName, new HashSet<User>() },
            { Constants.UserRoleName, new HashSet<User>() },
        };

        public static async void SeedDatabase(this IApplicationBuilder app)
        {
            var serviceScoreFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var scope = serviceScoreFactory.CreateScope();
            using (scope)
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // add the roles if they dont exist
                await SeedRolesAsync(roleManager);

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                // add default administrator if it doesnt exist
                await SeedUsers(userManager);
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in roles.Keys)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var reuslt = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedUsers(UserManager<User> userManager)
        {
            foreach (var role in defaultUsers.Keys)
            {
                foreach (var currentUser in defaultUsers[role])
                {
                    string username = currentUser.Item1;
                    string email = currentUser.Item2;
                    string password = currentUser.Item3;

                    var user = await userManager.FindByNameAsync(username);

                    if (user == null)
                    {
                        user = new User()
                        {
                            UserName = username,
                            Email = email,
                            RegisterDate = DateTime.Now
                        };

                        var result = await userManager.CreateAsync(user, password);
                        result = await userManager.AddToRoleAsync(user, role);
                    }
                }
            }
        }
    }
}
