namespace QuizSystem.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IUserResultsService
    {
        IEnumerable<T> GetAllContests<T>(string userId);

        T GetContest<T>(string userId, string userResultId);
    }
}
