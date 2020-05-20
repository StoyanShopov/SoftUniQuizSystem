using System.Collections.Generic;

namespace QuizSystem.Web.ViewModels.UserResults.ViewModels
{
    public class UserQuizResultViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<UserQuestionReslutViewModel> UserQuestionReslut { get; set; }
    }
}