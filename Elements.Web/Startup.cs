﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Elements.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Elements.Web.Areas.Identity.Services;
using Elements.Models;
using Elements.Web.Common;
using AutoMapper;
using Elements.Services.Admin.Interfaces;
using Elements.Services.Admin;
using Microsoft.AspNetCore.Routing;
using Elements.Services.Public.Interfaces;
using Elements.Services.Public;
using Elements.Common;
using System;

namespace Elements.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // set db context
            services.AddDbContext<ElementsContext>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.PolicyRequireAdminRole, policy => policy.RequireRole(Constants.AdminRoleName, Constants.CreatorRoleName));
                options.AddPolicy(Constants.PolicyRequireAdminDevRole, policy => policy.RequireRole(Constants.AdminRoleName, Constants.CreatorRoleName, Constants.DevRoleName));
            });

            services
                .AddAuthentication()
                .AddCookie();

            // set up app to use User and IdentityRole
            services.AddIdentity<User, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ElementsContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password = new PasswordOptions()
                {
                    RequireDigit = false,
                    RequiredLength = 4,
                    RequiredUniqueChars = 1,
                    RequireLowercase = false,
                    RequireUppercase = false,
                    RequireNonAlphanumeric = false
                };

                options.SignIn.RequireConfirmedEmail = false;
            });

            services.AddSingleton<IEmailSender, SendGridEmailSender>();

            services.AddAutoMapper();

            RegisterServiceLayer(services);

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddMvc()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddRazorPagesOptions(options =>
            {
                options.AllowAreas = true;

                options.Conventions.AuthorizeAreaFolder(Constants.AdminAreaName, "/Admin", Constants.PolicyRequireAdminRole);
                options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                options.Conventions.AddPageRoute("/News", "");

            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;

                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IDateTimeService dateTimeService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.SeedDatabase(dateTimeService);

            app.UseMvc(routes =>
            {
                // Areas support
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                // Default routing
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void RegisterServiceLayer(IServiceCollection services)
        {
            services.AddScoped<IManageCategoriesService, ManageCategoriesService>();
            services.AddScoped<IManageUsersService, ManageUsersService>();
            services.AddScoped<IManageNewsService, ManageNewsService>();

            // public services
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICharacterService, CharacterService>();
        }
    }
}
