namespace QuizSystem.Web.ViewModels.Quizzes.InputModels
{
    using System.Collections.Generic;

    public class QuestionInputModel
    {
        public string QuestionId { get; set; }

        public IEnumerable<CheckBoxInputModel> Answers { get; set; }
    }
}
