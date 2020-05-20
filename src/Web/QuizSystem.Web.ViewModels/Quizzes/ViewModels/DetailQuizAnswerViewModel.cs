namespace QuizSystem.Web.ViewModels.Quizzes.ViewModels
{
    using QuizSystem.Data.Models;
    using QuizSystem.Services.Mapping;

    public class DetailQuizAnswerViewModel : IMapFrom<Answer>
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public bool IsChecked { get; set; }
    }
}
