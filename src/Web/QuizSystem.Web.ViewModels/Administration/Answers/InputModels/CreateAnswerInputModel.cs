namespace QuizSystem.Web.ViewModels.Administration.Answers.InputModels
{
    public class CreateAnswerInputModel
    {
        public string Text { get; set; }

        public bool IsFreeAnswer { get; set; }

        public string QuestionId { get; set; }

        public bool IsCorrect { get; set; }
    }
}
