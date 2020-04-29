namespace QuizSystem.Web.ViewModels.Administration.Questions.InputModels
{
    using Microsoft.AspNetCore.Http;

    public class CreateQuestionInputModel
    {
        public string Text { get; set; }

        public IFormFile Image { get; set; }

        public string QuizId { get; set; }
    }
}
