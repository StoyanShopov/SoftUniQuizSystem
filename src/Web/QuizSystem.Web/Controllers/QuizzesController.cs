namespace QuizSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using QuizSystem.Services.Data.Contracts;
    using QuizSystem.Web.ViewModels.Administration.Quizzes.ViewModels;

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
    }
}
