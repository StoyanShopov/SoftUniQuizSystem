﻿namespace QuizHut.Web.ViewModels.Groups
{
    using AutoMapper;
    using QuizHut.Data.Models;
    using QuizHut.Services.Mapping;

    public class GroupListViewModel : IMapFrom<Group>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int StudentsCount { get; set; }

        public int EventsCount { get; set; }

        public string CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Group, GroupListViewModel>()
                .ForMember(
                    x => x.CreatedOn,
                    opt => opt.MapFrom(x => x.CreatedOn.ToString("dd/MM/yyyy")))
                .ForMember(
                    x => x.StudentsCount,
                    opt => opt.MapFrom(x => x.StudentstGroups.Count))
                .ForMember(
                    x => x.EventsCount,
                    opt => opt.MapFrom(x => x.EventsGroups.Count));
        }
    }
}
