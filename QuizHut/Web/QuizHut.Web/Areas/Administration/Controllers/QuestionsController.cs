namespace QuizHut.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using QuizHut.Common;
    using QuizHut.Services.Questions;
    using QuizHut.Web.Common;
    using QuizHut.Web.Infrastructure.Filters;
    using QuizHut.Web.ViewModels.Questions;

    public class QuestionsController : AdministrationController
    {
        private readonly IQuestionsService questionService;
        private readonly IWebHostEnvironment env;

        public QuestionsController(IQuestionsService questionService, IWebHostEnvironment env)
        {
            this.questionService = questionService;
            this.env = env;
        }

        [HttpGet]
        [SetDashboardRequestToTrueInViewDataActionFilterAttribute]
        public IActionResult QuestionInput(string id)
        {
            if (id != null)
            {
                this.HttpContext.Session.SetString(Constants.QuizSeesionId, id);
            }

            return this.View();
        }

        [HttpPost]
        [ModelStateValidationActionFilterAttribute]
        public async Task<IActionResult> AddNewQuestion(QuestionInputModel model)
        {
            var quizId = this.HttpContext.Session.GetString(Constants.QuizSeesionId);
            var questionId = await this.questionService.CreateQuestionAsync(quizId, model.Text);
            this.HttpContext.Session.SetString(Constants.CurrentQuestionId, questionId);
            return this.RedirectToAction("AnswerInput", "Answers");
        }

        [HttpPost]
        [ModelStateValidationActionFilterAttribute]
        public Task<IActionResult> ImportQuestions([FromForm(Name = "file_1")] IFormFile file, string id)
        {
            var dir = this.env.ContentRootPath;

            this.questionService.ImportQuestions(id, file, dir);

            return null;
        }

        [HttpGet]
        [SetDashboardRequestToTrueInViewDataActionFilterAttribute]
        public async Task<IActionResult> EditQuestionInput(string id)
        {
            var model = await this.questionService.GetByIdAsync<QuestionInputModel>(id);
            return this.View(model);
        }

        [HttpPost]
        [ModelStateValidationActionFilterAttribute]
        public async Task<IActionResult> Edit(QuestionInputModel model)
        {
            await this.questionService.Update(model.Id, model.Text);
            var page = this.HttpContext.Session.GetInt32(GlobalConstants.PageToReturnTo);
            return this.RedirectToAction("Display", "Quizzes", new { page });
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.questionService.DeleteQuestionByIdAsync(id);

            return this.RedirectToAction("Display", "Quizzes");
        }
    }
}
