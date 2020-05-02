namespace QuizSystem.Web.ViewModels.Quizzes.ViewModels
{
    using System.Collections.Generic;

    using QuizSystem.Data.Models;
    using QuizSystem.Services.Mapping;

    public class DetailQuizViewModel : IMapFrom<Quiz>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<DetailQuizQuestionViewModel> Questions { get; set; }
    }
}
