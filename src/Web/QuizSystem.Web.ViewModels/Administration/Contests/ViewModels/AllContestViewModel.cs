namespace QuizSystem.Web.ViewModels.Administration.Contests.ViewModels
{
    using QuizSystem.Data.Models;
    using QuizSystem.Services.Mapping;

    public class AllContestViewModel : IMapFrom<Contest>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
