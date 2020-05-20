namespace QuizSystem.Web.ViewModels.UserResults.ViewModels
{
    using System;

    using QuizSystem.Data.Models;
    using QuizSystem.Services.Mapping;

    public class UserCompletedContestViewModel : IMapFrom<UserResult>
    {
        public string Id { get; set; }

        public string ContestName { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime FinishedOn { get; set; }
    }
}
