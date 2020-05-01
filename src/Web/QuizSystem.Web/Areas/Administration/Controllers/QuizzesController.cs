namespace QuizSystem.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using QuizSystem.Services.Data.Contracts;
    using QuizSystem.Web.ViewModels.Administration.Quizzes.InputModels;
    using QuizSystem.Web.ViewModels.Administration.Quizzes.ViewModels;

    public class QuizzesController : AdministrationController
    {
        private readonly IQuizzesService quizzesService;

        public QuizzesController(IQuizzesService quizzesService)
        {
            this.quizzesService = quizzesService;
        }

        public IActionResult All()
        {
            var allQuizzes = this.quizzesService.GetAll<AllQuizViewModel>();

            return this.View(allQuizzes);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateQuizInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var quizId = await this.quizzesService.CreateAsync(inputModel);

            return this.RedirectToAction("Create", "Questions", quizId);
        }

        public IActionResult Details(string quizId)
        {
            var quiz = this.quizzesService.GetById<DetailQuizViewModel>(quizId);


            return this.View(quiz);
        }

        [HttpPost]
        public async Task<IActionResult> ImportQuestions([FromForm(Name = "file_1")] IFormFile file, string id)
        {
            var quizId = await this.quizzesService.ImportQuestionsAsync(id, file);
            return this.RedirectToAction("Details", new { quizId });
        }
    }
}
