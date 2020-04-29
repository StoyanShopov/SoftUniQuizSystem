namespace QuizSystem.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using QuizSystem.Services.Data.Contracts;
    using QuizSystem.Web.ViewModels.Administration.Questions.InputModels;

    public class QuestionsController : AdministrationController
    {
        private readonly IQuestionsService questionsService;

        public QuestionsController(IQuestionsService questionsService)
        {
            this.questionsService = questionsService;
        }

        public IActionResult Create(string quizId)
        {
            if (quizId == null)
            {
                this.RedirectToAction("Create", "Quizzes");
            }

            this.ViewData["QuizId"] = quizId;

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateQuestionInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var questionId = await this.questionsService.CreateAsync(inputModel);

            return this.RedirectToAction("Create", "Answers", questionId);
        }
    }
}
