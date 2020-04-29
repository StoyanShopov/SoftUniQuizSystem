namespace QuizSystem.Data.Models
{
    using System;

    using QuizSystem.Data.Common.Models;

    public class Answer : BaseDeletableModel<string>
    {
        public Answer()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string QuestionId { get; set; }

        public Question Question { get; set; }

        public string Text { get; set; }

        public bool IsCorrect { get; set; }

        public bool IsFreeAnswer { get; set; }
    }
}
