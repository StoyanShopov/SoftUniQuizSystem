namespace QuizSystem.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using QuizSystem.Services.Data.Contracts;
    using QuizSystem.Web.ViewModels.Administration.Quizzes.ViewModels;
    using ViewModels.Quizzes.InputModels;

    [Authorize]
    public class QuizzesController : BaseController
    {
        private readonly IQuizzesService quizzesService;

        public QuizzesController(IQuizzesService quizzesService)
        {
            this.quizzesService = quizzesService;
        }

        public IActionResult Index(string quizId)
        {
            var quiz = this.quizzesService
                .GetById<DetailQuizViewModel>(quizId);

            return this.View(quiz);
        }


        [HttpPost]
        public IActionResult Submit(DetailQuizViewModel model)
        {
            ;
            //var points = await this.quizzesService.SubmitAsync(model);

            return this.View();
        }
    }
}
