namespace QuizSystem.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using QuizSystem.Data.Common.Repositories;
    using QuizSystem.Data.Models;
    using QuizSystem.Services.Data.Contracts;
    using QuizSystem.Web.ViewModels.Administration.Answers.InputModels;

    public class AnswersController : AdministrationController
    {
        private readonly IAnswersService answersService;

        public AnswersController(IAnswersService answersService)
        {
            this.answersService = answersService;
        }

        public IActionResult Create(string questionId)
        {
            if (questionId == null)
            {
                return this.RedirectToAction("Create", "Questions");
            }

            this.ViewData["QuestionId"] = questionId;

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAnswerInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.answersService.CreateAsync(inputModel);

            return this.RedirectToAction("Create", new { questionId = inputModel.QuestionId });
        }
    }
}
