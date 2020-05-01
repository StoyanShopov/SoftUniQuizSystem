namespace QuizSystem.Web.ViewModels.Quizzes.InputModels
{
    using System.Collections.Generic;

    public class QuizSubmitInputModel
    {
        public string ContestId { get; set; }

        public string QuizId { get; set; }

        public IEnumerable<QuestionInputModel> Questions { get; set; }
    }
}
