﻿//<auto-generated/>
namespace QuizSystem.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using QuizSystem.Common;
    using QuizSystem.Data.Models;

    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LoginModel> logger;
        private readonly IHttpClientFactory clientFactory;

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager, IHttpClientFactory clientFactory)
        {
            this.userManager = userManager;
            this.clientFactory = clientFactory;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            public string TimeZoneIana { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(this.ErrorMessage))
            {
                this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
            }

            returnUrl = returnUrl ?? this.Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            this.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= this.Url.Content("~/");

            if (this.ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

                //if user doesn't exists
                var userExists = await CheckIfUserExistsAsync(this.Input.Username, this.Input.Password);

                if (!userExists)
                {
                    return this.Page();
                }

                //RegisterUserIfHeIsNew
                await RegisterUserIfIsNew(this.Input.Username, this.Input.Password);

                var result = await this.signInManager.PasswordSignInAsync(this.Input.Username, this.Input.Password, this.Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await this.userManager.FindByNameAsync(this.Input.Username);
                    var roles = await this.userManager.GetRolesAsync(user);

                    if (roles.Count > 0)
                    {
                        returnUrl = this.Url.Content("~/Administration/Dashboard/Index");
                    }
                    else
                    {
                        returnUrl = this.Url.Content("~/Contests/Index");
                    }

                    this.logger.LogInformation("User logged in.");
                   
                    return this.LocalRedirect(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    this.logger.LogWarning("User account locked out.");
                    return this.RedirectToPage("./Lockout");
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return this.Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }

        private async Task RegisterUserIfIsNew(string inputUsername, string inputPassword)
        {
            var user = await this.userManager.FindByNameAsync(this.Input.Username);

            if (user != null)
            {
                return;
            }

            user = new ApplicationUser { UserName = this.Input.Username };
            await this.userManager.CreateAsync(user, this.Input.Password);
        }

        private async Task<bool> CheckIfUserExistsAsync(string inputUsername, string inputPassword)
        {
            var targetUrl = "https://judge.softuni.bg/Account/Login";
            var request = new HttpRequestMessage(HttpMethod.Get, targetUrl);

            var client = clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            var htmlResult = await response.Content.ReadAsStringAsync();
            var requestVerificationToken = ExtractRequestVerificationToken(htmlResult);
          
            var keyValuePairCollection = new Dictionary<string, string>
            {
                { "UserName", inputUsername },
                { "Password", inputPassword },
                { "__RequestVerificationToken", requestVerificationToken },
            };
            var content = new FormUrlEncodedContent(keyValuePairCollection);

            clientFactory.CreateClient();
            response = await client.PostAsync(targetUrl, content);
            htmlResult = await response.Content.ReadAsStringAsync();

            // TODO: Fix it!!!
            if (htmlResult.Contains("action=\"/Account/LogOff\"")
                && htmlResult.Contains($"<a class=\"text-primary\" href=\"/Users/Profile\" title=\"Settings\">Hello, {inputUsername}!</a>"))
            {
                return true;
            }

            return false;
        }

        private string ExtractRequestVerificationToken(string content)
        {
            var pattern = "__RequestVerificationToken.*value=\"[A-Za-z0-9_-]+\"";
            var valuePattern = "[A-Za-z0-9_-]{30,}";
            var extracted = Regex.Match(content, pattern).Value;
            var value = Regex.Match(extracted, valuePattern).Value;

            return value;
        }
    }
}
