namespace QuizSystem.Web.Common
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using QuizSystem.Services.Data.Contracts;
    using QuizSystem.Web.ViewModels.Administration.Quizzes.ViewModels;

    public class SelectListGenerator
    {
        public static IEnumerable<SelectListItem> GetAllQuizzes(IQuizzesService quizzesService)
        {
            var quizzes = quizzesService.GetAll<DetailQuizViewModel>();

            var groups = new List<SelectListGroup>();

            foreach (var category in quizzes)
            {
                groups.Add(new SelectListGroup { Name = category.Name });
            }

            var result = quizzes.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name,
            });

            return result;
        }
    }
}
