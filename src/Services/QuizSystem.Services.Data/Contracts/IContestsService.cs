namespace QuizSystem.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using QuizSystem.Web.ViewModels.Administration.Contests.InputModels;

    public interface IContestsService
    {
        Task<string> CreateAsync(CreateContestInputModel inputModel);

        IEnumerable<T> GetAll<T>();

        T GetById<T>(string contestId);

        string GetContestIdByPassword(string password);

        bool IsAvailable(string password);

        Task AssignUserToContestAsync(string userId, string contestId);

        string GetQuizIdByContestId(string contestId);
    }
}
