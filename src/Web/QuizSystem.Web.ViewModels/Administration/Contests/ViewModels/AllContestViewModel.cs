namespace QuizSystem.Web.ViewModels.Administration.Contests.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices.ComTypes;
    using AutoMapper;
    using QuizSystem.Data.Models;
    using QuizSystem.Services.Mapping;

    public class AllContestViewModel : IMapFrom<Contest>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string QuizId { get; set; }

        public string Password { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        //public int UserContests { get; set; }

        //public void CreateMappings(IProfileExpression configuration)
        //{
        //    configuration.CreateMap<Contest, AllContestViewModel>()
        //        .ForMember(c => c.UserContests, y => y.MapFrom(s => s.UserContests.Count(x => x.ContestId == this.Id)));
        //}
    }
}
