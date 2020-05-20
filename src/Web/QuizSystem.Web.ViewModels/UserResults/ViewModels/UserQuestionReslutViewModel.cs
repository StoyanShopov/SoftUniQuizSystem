using System.Collections.Generic;

namespace QuizSystem.Web.ViewModels.UserResults.ViewModels
{
    public class UserQuestionReslutViewModel
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public ICollection<UserAnswerViewModel> UserAnswers { get; set; }
    }
}