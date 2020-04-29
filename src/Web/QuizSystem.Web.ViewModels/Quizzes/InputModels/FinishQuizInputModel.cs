namespace QuizSystem.Web.ViewModels.Quizzes.InputModels
{
    using System.Collections.Generic;

    public class FinishQuizInputModel
    {
        public string QuizId { get; set; }

        public IEnumerable<FinishQuizQuestionInputModel> Questions { get; set; }
    }
}
