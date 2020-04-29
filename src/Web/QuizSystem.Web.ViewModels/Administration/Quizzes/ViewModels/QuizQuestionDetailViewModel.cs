namespace QuizSystem.Web.ViewModels.Administration.Quizzes.ViewModels
{
    using System.Collections.Generic;

    using QuizSystem.Data.Models;
    using QuizSystem.Services.Mapping;

    public class QuizQuestionDetailViewModel : IMapFrom<Question>
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public IEnumerable<QuizQuestionAnswerDetailViewModel> Answers { get; set; }
    }
}
