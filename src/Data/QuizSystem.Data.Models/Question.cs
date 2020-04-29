namespace QuizSystem.Data.Models
{
    using System;
    using System.Collections.Generic;

    using QuizSystem.Data.Common.Models;

    public class Question : BaseDeletableModel<string>
    {
        public Question()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Answers = new HashSet<Answer>();
        }

        public string Text { get; set; }

        public string ImagePath { get; set; }

        public string QuizId { get; set; }

        public Quiz Quiz { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}
