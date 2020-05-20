namespace QuizSystem.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using QuizSystem.Data.Common.Repositories;
    using QuizSystem.Data.Models;
    using QuizSystem.Services.Data.Contracts;
    using QuizSystem.Services.Mapping;

    public class UserResultsService : IUserResultsService
    {
        private readonly IDeletableEntityRepository<UserResult> userResultRepository;

        public UserResultsService(IDeletableEntityRepository<UserResult> userResultRepository)
        {
            this.userResultRepository = userResultRepository;
        }

        public IEnumerable<T> GetAllContests<T>(string userId)
            => this.userResultRepository.All()
                   .Where(x => x.ApplicationUserId == userId)
                   .To<T>()
                   .ToList();

        public T GetContest<T>(string userId, string userResultId)
            => this.userResultRepository.All()
                       .Where(x => x.ApplicationUserId == userId && x.Id == userResultId)
                       .To<T>()
                       .FirstOrDefault();
    }
}
