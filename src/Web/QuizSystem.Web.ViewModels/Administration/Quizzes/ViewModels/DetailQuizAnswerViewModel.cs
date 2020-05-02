namespace QuizSystem.Web.ViewModels.Administration.Quizzes.ViewModels
{
    using AutoMapper;
    using QuizSystem.Data.Models;
    using QuizSystem.Services.Mapping;

    public class DetailQuizAnswerViewModel : IMapFrom<Answer>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public bool Checked { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Answer, DetailQuizAnswerViewModel>()
                .ForMember(x => x.Checked, y => y.MapFrom(t => t.IsCorrect));
        }
    }
}
