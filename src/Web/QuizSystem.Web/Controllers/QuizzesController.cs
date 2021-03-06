﻿namespace QuizSystem.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using QuizSystem.Services.Data.Contracts;
    using QuizSystem.Web.ViewModels.Quizzes.ViewModels;

    [Authorize]
    public class QuizzesController : BaseController
    {
        private readonly IQuizzesService quizzesService;
        private readonly IContestsService contestsService;

        public QuizzesController(
            IQuizzesService quizzesService,
            IContestsService contestsService)
        {
            this.quizzesService = quizzesService;
            this.contestsService = contestsService;
        }

        public IActionResult Start(string contestId)
        {
            var contest = this.contestsService
                .GetById<ContestViewModel>(contestId);

            return this.View(contest);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(ContestViewModel model)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.quizzesService.SubmitAsync(model, userId);

            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult Result()
        {
            return this.View();
        }
    }
}
