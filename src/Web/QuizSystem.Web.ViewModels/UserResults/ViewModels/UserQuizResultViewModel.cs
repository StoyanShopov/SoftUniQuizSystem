namespace QuizSystem.Web.ViewModels.UserResults.ViewModels
{
    using System.Collections.Generic;

    public class UserQuizResultViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<UserQuestionReslutViewModel> UserQuestionReslut { get; set; }
    }
}
