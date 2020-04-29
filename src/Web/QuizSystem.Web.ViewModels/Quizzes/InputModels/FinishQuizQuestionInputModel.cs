namespace QuizSystem.Web.ViewModels.Quizzes.InputModels
{
    using System.Collections.Generic;

    public class FinishQuizQuestionInputModel
    {
        public string QuestionId { get; set; }

        public IEnumerable<FinishQuizQuestionAnswerCheckBoxInputModel> Answers { get; set; }
    }
}
