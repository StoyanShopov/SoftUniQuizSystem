namespace QuizSystem.Data.Models
{
    using System;
    using System.Collections.Generic;

    using QuizSystem.Data.Common.Models;

    public class UserResult : BaseDeletableModel<string>
    {
        public UserResult()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public string ContestId { get; set; }

        public Contest Contest { get; set; }

        public DateTime FinishedOn { get; set; }

        public ICollection<UserQuestion> UserQuestions { get; set; }
    }
}
