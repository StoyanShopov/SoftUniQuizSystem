using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizSystem.Services.Data.Contracts;
using QuizSystem.Web.ViewModels.Administration.Quizzes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuizSystem.Web.Controllers
{
    [Authorize]
    public class ContestsController : BaseController
    {
        private readonly IContestsService contestsService;
        private readonly IQuizzesService quizzesService;

        public ContestsController(IContestsService contestsService, IQuizzesService quizzesService)
        {
            this.contestsService = contestsService;
            this.quizzesService = quizzesService;
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
