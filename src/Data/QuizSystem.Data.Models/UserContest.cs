namespace QuizSystem.Data.Models
{
    using System;

    using QuizSystem.Data.Common.Models;

    public class UserContest : BaseDeletableModel<string>
    {
        public UserContest()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public string ContestId { get; set; }

        public Contest Contest { get; set; }

        public int Points { get; set; }
    }
}
