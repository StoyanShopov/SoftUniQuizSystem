namespace QuizSystem.Data.Models
{
    using System;
    using System.Collections.Generic;

    using QuizSystem.Data.Common.Models;

    public class Quiz : BaseDeletableModel<string>
    {
        public Quiz()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Questions = new HashSet<Question>();
            this.Contests = new HashSet<Contest>();
        }

        public string Name { get; set; }

        public ICollection<Question> Questions { get; set; }

        public ICollection<Contest> Contests { get; set; }
    }
}
