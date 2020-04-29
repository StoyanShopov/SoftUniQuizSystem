namespace QuizSystem.Web.Controllers
{
    using System.Diagnostics;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using QuizSystem.Services.Data.Contracts;
    using QuizSystem.Web.ViewModels;
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

        //[HttpPost]
        //public IActionResult Finish(FinishQuizInputModel model)
        //{

        //    ;

        //    return this.View();
        //}
    }
}
