namespace QuizHut.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    using QuizHut.Common;
    using QuizHut.Web.Infrastructure.Filters;
    using QuizHut.Web.ViewModels;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return this.View();
            }

            var isInRoleAdminOrTeacher = this.User.IsInRole(GlobalConstants.AdministratorRoleName) ||
                                         this.User.IsInRole(GlobalConstants.TeacherRoleName);

            if (isInRoleAdminOrTeacher)
            {
                return this.Redirect("/Administration/Home/Index");
            }

            return this.Redirect("/Students/Index");
        }

        [ChangeDefaoultLayoutActionFilterAttribute]
        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string code)
        {
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                StatusCode = code,
            };

            return this.View(errorViewModel);
        }

        public IActionResult StatusCode(string code)
        {
            return this.RedirectToAction("Error", new { code });
        }
    }
}
