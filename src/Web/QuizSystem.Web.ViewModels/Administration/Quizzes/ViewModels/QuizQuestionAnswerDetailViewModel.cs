namespace QuizSystem.Web.ViewModels.Administration.Quizzes.ViewModels
{
    using QuizSystem.Data.Models;
    using QuizSystem.Services.Mapping;

    public class QuizQuestionAnswerDetailViewModel : IMapFrom<Answer>
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public bool IsFreeAnswer { get; set; }
    }
}
