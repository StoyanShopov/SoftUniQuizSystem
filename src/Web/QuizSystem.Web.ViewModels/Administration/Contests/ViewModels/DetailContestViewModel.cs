namespace QuizSystem.Web.ViewModels.Administration.Contests.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using QuizSystem.Data.Models;
    using QuizSystem.Services.Mapping;

    public class DetailContestViewModel : IMapFrom<Contest>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string QuizId { get; set; }

        public string Password { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}
