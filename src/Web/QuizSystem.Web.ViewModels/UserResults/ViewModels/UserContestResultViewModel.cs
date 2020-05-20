using System;
using System.Collections.Generic;
using System.Text;

namespace QuizSystem.Web.ViewModels.UserResults.ViewModels
{
    public class UserContestResultViewModel
    {
        public string ContestName { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime FinishedOn { get; set; }

        public ICollection<UserQuizResultViewModel> UserQuizResults { get; set; }
    }
}
