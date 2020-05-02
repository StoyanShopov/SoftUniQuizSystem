﻿namespace QuizSystem.Web.ViewModels.Quizzes.ViewModels
{
    using QuizSystem.Data.Models;
    using QuizSystem.Services.Mapping;

    public class ContestViewModel : IMapFrom<Contest>
    {
        public string Id { get; set; }

        public DetailQuizViewModel Quiz { get; set; }
    }
}
