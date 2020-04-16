﻿namespace QuizHut.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using QuizHut.Common;
    using QuizHut.Common.Hubs;
    using QuizHut.Data.Models;
    using QuizHut.Services.Questions;
    using QuizHut.Services.Quizzes;
    using QuizHut.Web.Common;
    using QuizHut.Web.Infrastructure.Filters;
    using QuizHut.Web.ViewModels.Questions;
    using QuizHut.Web.ViewModels.Quizzes;
    using Rotativa.AspNetCore;

    public class QuizzesController : AdministrationController
    {
        private const int PerPageDefaultValue = 5;
        private const int QuestionsPerPageDefaultValue = 1;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IQuizzesService quizService;
        private readonly IQuestionsService questionsService;
        private readonly IHubContext<QuizHub> hub;

        public QuizzesController(
            UserManager<ApplicationUser> userManager,
            IQuizzesService quizService,
            IQuestionsService questionsService,
            IHubContext<QuizHub> hub)
        {
            this.userManager = userManager;
            this.quizService = quizService;
            this.questionsService = questionsService;
            this.hub = hub;
        }

        public IActionResult DetailsInput()
        {
            return this.View();
        }

        [HttpPost]
        [ModelStateValidationActionFilterAttribute]
        public async Task<IActionResult> DetailsInput(InputQuizViewModel model)
        {
            var quizWithSamePasswordId = await this.quizService.GetQuizIdByPasswordAsync(model.Password);
            if (quizWithSamePasswordId != null)
            {
                return this.View(model);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            model.CreatorId = user.Id;
            model.PasswordIsValid = true;
            var quizId = await this.quizService.CreateQuizAsync(model.Name, model.Description, model.Timer, model.CreatorId, model.Password);
            this.HttpContext.Session.SetString(Constants.QuizSeesionId, quizId);
            await this.hub.Clients.Group(GlobalConstants.AdministratorRoleName).SendAsync("NewQuizUpdate", user.UserName, model.Name);
            return this.RedirectToAction("QuestionInput", "Questions");
        }

        [HttpGet]
        [SetDashboardRequestToTrueInViewDataActionFilterAttribute]
        public async Task<IActionResult> Display(string id, int? page, int countPerPage = QuestionsPerPageDefaultValue)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            {
                id = this.HttpContext.Session.GetString(Constants.QuizSeesionId);
            }

            this.HttpContext.Session.SetString(Constants.QuizSeesionId, id);

            if (page == null)
            {
                page = this.HttpContext.Session.GetInt32(GlobalConstants.PageToReturnTo) ?? 1;
            }

            var pagesCount = 0;
            var quizDetails = await this.quizService.GetQuizByIdAsync<QuizDetailsViewModel>(id);
            var model = new QuizDetailsPagingModel()
            {
                Details = quizDetails,
                CurrentPage = (int)page,
                PagesCount = pagesCount,
            };

            var questionsCount = this.questionsService.GetAllByQuizIdCount(id);

            if (questionsCount > 0)
            {
                var question = await this.questionsService.GetQuestionByQuizIdAndNumberAsync<QuestionViewModel>(id, (int)page);
                model.Question = question;
                model.PagesCount = questionsCount;
            }

            this.HttpContext.Session.SetInt32(GlobalConstants.PageToReturnTo, (int)page);
            return this.View(model);
        }

        [ClearDashboardRequestInSessionActionFilterAttribute]
        public async Task<IActionResult> AllQuizzesCreatedByTeacher(int page = 1, int countPerPage = PerPageDefaultValue)
        {
            var userId = this.userManager.GetUserId(this.User);
            var allQuizzesCreatedByTeacher = this.quizService.GetAllQuizzesCount(userId);
            var pagesCount = 0;

            var model = new QuizzesAllListingViewModel()
            {
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            if (allQuizzesCreatedByTeacher > 0)
            {
                pagesCount = (int)Math.Ceiling(allQuizzesCreatedByTeacher / (decimal)countPerPage);
                var quizzes = await this.quizService.GetAllPerPageAsync<QuizListViewModel>(page, countPerPage, userId);
                model.Quizzes = quizzes;
                model.PagesCount = pagesCount;
            }

            return this.View(model);
        }

        [HttpPost]
        [ModelStateValidationActionFilterAttribute]
        public IActionResult AllQuizzesCreatedByTeacher(QuizzesAllListingViewModel model)
        {
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Start(PasswordInputViewModel model)
        {
            if (model.Password == null)
            {
                return this.RedirectToAction("Index", "Home", new
                {
                    password = model.Password,
                    area = GlobalConstants.Administration,
                    errorText = GlobalConstants.ErrorMessages.EmptyPasswordField,
                });
            }

            var id = await this.quizService.GetQuizIdByPasswordAsync(model.Password);
            if (id == null)
            {
                return this.RedirectToAction("Index", "Home", new
                {
                    password = model.Password,
                    area = GlobalConstants.Administration,
                    errorText = string.Format(GlobalConstants.ErrorMessages.QuizNotFound, model.Password),
                });
            }

            return this.RedirectToAction("Start", "Quizzes", new { area = string.Empty, password = model.Password });
        }

        public async Task<IActionResult> DeleteQuiz(string id)
        {
            await this.quizService.DeleteByIdAsync(id);
            return this.RedirectToAction("AllQuizzesCreatedByTeacher", "Quizzes", new { area = "Administration" });
        }

        [HttpGet]
        [SetDashboardRequestToTrueInViewDataActionFilterAttribute]
        public async Task<IActionResult> EditDetailsInput(string id)
        {
            var editModel = await this.quizService.GetQuizByIdAsync<EditDetailsViewModel>(id);
            editModel.PasswordIsValid = true;

            return this.View(editModel);
        }

        [HttpPost]
        [ModelStateValidationActionFilterAttribute]
        public async Task<IActionResult> EditDetailsInput(EditDetailsViewModel model)
        {
            var quizWithSamePasswordId = await this.quizService.GetQuizIdByPasswordAsync(model.Password);
            if (quizWithSamePasswordId != null && quizWithSamePasswordId != model.Id)
            {
                model.PasswordIsValid = false;
                return this.View(model);
            }

            await this.quizService.UpdateAsync(model.Id, model.Name, model.Description, model.Timer, model.Password);

            return this.RedirectToAction("Display", new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> PDFExport(string id)
        {
            var quizModel = await this.quizService.GetQuizByIdAsync<QuizPDFViewModel>(id);

            return new ViewAsPdf("PDFExport", quizModel)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageMargins = { Left = 20, Bottom = 20, Right = 20, Top = 20 },
            };
        }
    }
}
