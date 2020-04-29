using QuizSystem.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizSystem.Data.Models
{
    public class UserContest : BaseDeletableModel<string>
    {
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public string ContestId { get; set; }

        public Contest Contest { get; set; }
    }
}
