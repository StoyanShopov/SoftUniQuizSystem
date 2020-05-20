namespace QuizSystem.Web.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using QuizSystem.Services.Data.Contracts;
    using QuizSystem.Web.ViewModels.Administration.Quizzes.ViewModels;
    using QuizSystem.Web.ViewModels.UserResults.ViewModels;

    [Authorize]
    public class ResultsController : BaseController
    {
        private readonly IUserResultsService userResultsService;

        public ResultsController(IUserResultsService userResultsService)
        {
            this.userResultsService = userResultsService;
        }

        public IActionResult Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userContest = this.userResultsService
                .GetAllContests<UserCompletedContestViewModel>(userId);

            return this.View(userContest);
        }

        public IActionResult Details(string userResultId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userContest = this.userResultsService
                .GetContest<UserQuizResultViewModel>(userId, userResultId);

            return this.View(userContest);
        }
    }
}
