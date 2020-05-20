namespace QuizSystem.Web.ViewModels.UserResults.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class UserContestResultViewModel
    {
        public string ContestName { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime FinishedOn { get; set; }

        public ICollection<UserQuizResultViewModel> UserQuizResults { get; set; }
    }
}
