namespace QuizSystem.Web.ViewModels.Administration.Contests.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using QuizSystem.Data.Models;
    using QuizSystem.Services.Mapping;

    public class AllContestViewModel : IMapFrom<Contest>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
