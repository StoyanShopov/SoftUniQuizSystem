namespace QuizSystem.Data.Models
{
    using System;

    using QuizSystem.Data.Common.Models;

    public class UserAnswer : BaseDeletableModel<string>
    {
        public UserAnswer()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string QuestionId { get; set; }

        public Question Question { get; set; }

        public string Text { get; set; }

        public string SelectedId { get; set; }
    }
}