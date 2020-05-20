namespace QuizSystem.Web.ViewModels.Administration.Contests.InputModels
{
    using System;

    public class EditContestInputModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string QuizId { get; set; }

        public string Password { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}
