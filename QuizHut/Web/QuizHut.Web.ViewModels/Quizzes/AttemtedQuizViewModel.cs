﻿namespace QuizHut.Web.ViewModels.Quizzes
{
    using System.Collections.Generic;

    using QuizHut.Data.Models;
    using QuizHut.Services.Mapping;

    public class AttemtedQuizViewModel : IMapFrom<Quiz>
    {
        public AttemtedQuizViewModel()
        {
            this.Questions = new List<AttemtedQuizQuestionViewModel>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Timer { get; set; }

        public IList<AttemtedQuizQuestionViewModel> Questions { get; set; }
    }
}
