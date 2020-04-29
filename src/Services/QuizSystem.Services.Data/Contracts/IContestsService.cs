namespace QuizSystem.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using QuizSystem.Web.ViewModels.Administration.Contests.InputModels;
    using QuizSystem.Web.ViewModels.Administration.Quizzes.ViewModels;

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
