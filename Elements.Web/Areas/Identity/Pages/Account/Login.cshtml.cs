using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Elements.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Elements.Services.Models.Account;

namespace Elements.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> signInManager;
        private readonly ILogger<LoginModel> logger;
        private readonly UserManager<User> userManager;

        public LoginModel(
            SignInManager<User> signInManager,
            ILogger<LoginModel> logger,
            UserManager<User> userManager)
        {
            this.signInManager = signInManager;
            this.logger = logger;
            this.userManager = userManager;
        }

        [BindProperty]
        public LoginUserModel LoginUserModel { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync())
                                    .ToList();

            this.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (this.ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await signInManager.PasswordSignInAsync(LoginUserModel.Username,
                    LoginUserModel.Password,
                    LoginUserModel.RememberMe,
                    lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await userManager.FindByNameAsync(LoginUserModel.Username);

                    if (user.IsRestricted)
                    {
                        this.logger.LogInformation(string.Format("Restricted user '{0}' tried to log in.", LoginUserModel.Username));

                        this.ModelState.AddModelError(string.Empty, $"This account is restricted until {user.RestrictionEndDate.ToString("MMM dd, YYYY hh:mm")}");
                        return this.Page();
                    }

                    this.logger.LogInformation(string.Format("{0} logged in.", LoginUserModel.Username));
                    return this.LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return this.RedirectToPage("./LoginWith2fa",
                        new
                        {
                            ReturnUrl = returnUrl,
                            RememberMe = LoginUserModel.RememberMe
                        });
                }
                if (result.IsLockedOut)
                {
                    this.logger.LogWarning(string.Format("{0} account locked out.", LoginUserModel.Username));
                    return this.RedirectToPage("./Lockout");
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return this.Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
