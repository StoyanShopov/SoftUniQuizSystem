namespace QuizSystem.Web.ViewModels.Administration.Quizzes.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using QuizSystem.Data.Models;
    using QuizSystem.Services.Mapping;

    public class AllQuizViewModel : IMapFrom<Quiz>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
