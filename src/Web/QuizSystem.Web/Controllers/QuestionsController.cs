using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizSystem.Data.Models;
using QuizSystem.Services.Data.Contracts;
using QuizSystem.Web.ViewModels.Questions.InputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizSystem.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionsService questionsService;
        private readonly UserManager<ApplicationUser> userManager;

        public QuestionsController(
            IQuestionsService questionsService,
            UserManager<ApplicationUser> userManager)
        {
            this.questionsService = questionsService;
            this.userManager = userManager;
        }

        //[HttpPost]
        //public async Task<ActionResult> Post(AnswerQuestionInputModel inputModel)
        //{
        //    var user = await this.userManager.GetUserAsync(null);
        //    user.quizz
        //}
    }
}
