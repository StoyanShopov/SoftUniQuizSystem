namespace QuizSystem.Web.Areas.Administration.Controllers
{
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

        public IActionResult All()
        {
            var allContests = this.contestsService.GetAll<AllContestViewModel>();

            return this.View(allContests);
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
            var contest = this.contestsService.GetById<DetailContestViewModel>(contestId);

            return this.View(contest);
        }
    }
}
