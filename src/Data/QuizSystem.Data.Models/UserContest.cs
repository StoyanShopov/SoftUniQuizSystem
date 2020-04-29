namespace QuizSystem.Data.Models
{
    using QuizSystem.Data.Common.Models;

    public class UserContest : BaseDeletableModel<string>
    {
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public string ContestId { get; set; }

        public Contest Contest { get; set; }
    }
}
