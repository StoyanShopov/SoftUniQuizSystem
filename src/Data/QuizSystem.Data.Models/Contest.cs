namespace QuizSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using QuizSystem.Data.Common.Models;

    public class Contest : BaseDeletableModel<string>
    {
        public Contest()
        {
            this.Id = Guid.NewGuid().ToString();
            this.UserContests = new HashSet<UserContest>();
        }

        public string Name { get; set; }

        public string QuizId { get; set; }

        public Quiz Quiz { get; set; }

        public string Password { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public ICollection<UserContest> UserContests { get; set; }
    }
}
