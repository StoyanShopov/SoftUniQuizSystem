using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizSystem.Services.Data.Contracts;
using QuizSystem.Web.ViewModels.Contests.InputModel;

namespace QuizSystem.Web.Controllers
{
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
        public async Task<IActionResult> Start(StarContestInputModel model)
        {
            var isValidContest = this.contestsService.IsAvailable(model.Password);

            if (!isValidContest)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var contestId = this.contestsService.GetContestIdByPassword(model.Password);

            await this.contestsService.AssignUserToContestAsync(userId, contestId);

            return this.RedirectToAction("Start", "Quizzes", new { contestId });
        }
    }
}
