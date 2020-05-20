namespace QuizSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using QuizSystem.Data.Common.Models;

    public class UserAnswer : BaseDeletableModel<string>
    {
        public UserAnswer()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Text { get; set; }

        public string SelectedId { get; set; }
    }
}
