namespace QuizSystem.Web.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using QuizSystem.Services.Data.Contracts;
    using QuizSystem.Web.ViewModels.Administration.Quizzes.ViewModels;

    [Authorize]
    public class ContestsController : BaseController
    {
        private readonly IContestsService contestsService;

        public ContestsController(IContestsService contestsService)
        {
            this.contestsService = contestsService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Start(StarContestInputModel model)
        {
            var isValidContest = this.contestsService.IsAvailable(model.Password);

            if (!isValidContest)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var contestId = this.contestsService.GetContestIdByPassword(model.Password);

            this.contestsService.AssignUserToContestAsync(userId, contestId);

            var quizId = this.contestsService.GetQuizIdByContestId(contestId);

            return this.RedirectToAction("Index", "Quizzes", new { quizId });
        }
    }
}
