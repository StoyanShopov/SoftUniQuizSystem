namespace QuizSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using QuizSystem.Services.Data.Contracts;
    using QuizSystem.Web.Common;
    using QuizSystem.Web.ViewModels.Administration.Contests.InputModels;
    using QuizSystem.Web.ViewModels.Administration.Contests.ViewModels;

    public class ContestsController : AdministrationController
    {
        private readonly IQuizzesService quizzesService;
        private readonly IContestsService contestsService;

        public ContestsController(IQuizzesService quizzesService, IContestsService contestsService)
        {
            this.quizzesService = quizzesService;
            this.contestsService = contestsService;
        }

        public IActionResult All(int page = 1, int perPage = 9)
        {
            var allContests = this.contestsService.GetAll<AllContestViewModel>(page, perPage).ToList();
            var totalContests = this.contestsService.TotalContests;
            var pagesCount = (int)Math.Ceiling(totalContests / (decimal)perPage);

            var model = new ByPageContestViewModel
            {
                Contests = allContests,
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(model);
        }

        public IActionResult Create()
        {
            this.ViewData["AllQuizzes"] = SelectListGenerator.GetAllQuizzes(this.quizzesService);

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateContestInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                this.ViewData["AllQuizzes"] = SelectListGenerator.GetAllQuizzes(this.quizzesService);

                return this.View(inputModel);
            }

            var contestId = await this.contestsService.CreateAsync(inputModel);

            return this.RedirectToAction("Details", contestId);
        }

        public IActionResult Details(string contestId)
        {
            this.ViewData["AllQuizzes"] = SelectListGenerator.GetAllQuizzes(this.quizzesService);

            var contest = this.contestsService.GetById<DetailContestViewModel>(contestId);

            return this.View(contest);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EditContestInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                this.ViewData["AllQuizzes"] = SelectListGenerator.GetAllQuizzes(this.quizzesService);

                return this.RedirectToAction("Details", new { contestId = model.Id });
            }

            await this.contestsService.EditAsync(model);

            return this.RedirectToAction("Details", new { contestId = model.Id });
        }

        public async Task<IActionResult> Delete(string contestId)
        {
            await this.contestsService.DeleteAsync(contestId);

            return this.RedirectToAction("All");
        }
    }
}
