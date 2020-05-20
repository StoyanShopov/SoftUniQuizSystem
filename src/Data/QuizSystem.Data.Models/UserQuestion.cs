namespace QuizSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using QuizSystem.Data.Common.Models;

    public class UserQuestion : BaseDeletableModel<string>
    {
        public UserQuestion()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string UserResultId { get; set; }

        public UserResult UserResult { get; set; }

        public string QuestionId { get; set; }

        public Question Question { get; set; }

        public ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
