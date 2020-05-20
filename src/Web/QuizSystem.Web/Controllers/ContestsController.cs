namespace QuizSystem.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using QuizSystem.Services.Data.Contracts;
    using QuizSystem.Web.ViewModels.Contests.InputModel;

    [Authorize]
    public class ContestsController : BaseController
    {
        private readonly IContestsService contestsService;

        public ContestsController(IContestsService contestsService)
        {
            this.contestsService = contestsService;
        }

        public IActionResult Start()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Start(StarContestInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var isValidContest = this.contestsService.IsAvailable(model.Password);

            if (!isValidContest)
            {
                return this.View();
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var contestId = this.contestsService.GetContestIdByPassword(model.Password);

            await this.contestsService.AssignUserToContestAsync(userId, contestId);

            return this.RedirectToAction("Start", "Quizzes", new { contestId });
        }
    }
}
