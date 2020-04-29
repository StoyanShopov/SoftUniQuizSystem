namespace QuizSystem.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using QuizSystem.Data.Common.Repositories;
    using QuizSystem.Data.Models;
    using QuizSystem.Services.Data.Contracts;
    using QuizSystem.Services.Mapping;
    using QuizSystem.Web.ViewModels.Administration.Contests.InputModels;

    public class ContestsService : IContestsService
    {
        private readonly IDeletableEntityRepository<Contest> contestRepository;
        private readonly IDeletableEntityRepository<UserContest> userContests;

        public ContestsService(
            IDeletableEntityRepository<Contest> contestRepository,
            IDeletableEntityRepository<UserContest> userContests)
        {
            this.contestRepository = contestRepository;
            this.userContests = userContests;
        }

        public async Task AssignUserToContestAsync(string userId, string contestId)
        {
            var userContest = new UserContest()
            {
                ApplicationUserId = userId,
                ContestId = contestId,
            };

            await this.userContests.AddAsync(userContest);
            await this.userContests.SaveChangesAsync();
        }

        public async Task<string> CreateAsync(CreateContestInputModel inputModel)
        {
            var contest = new Contest
            {
                Name = inputModel.Name,
                StartDateTime = inputModel.StartDateTime,
                EndDateTime = inputModel.EndDateTime,
                Password = inputModel.Password,
                QuizId = inputModel.QuizId,
            };

            await this.contestRepository.AddAsync(contest);
            await this.contestRepository.SaveChangesAsync();

            return contest.Id;
        }

        public IEnumerable<T> GetAll<T>()
            => this.contestRepository
                .All()
                .OrderByDescending(x => x.CreatedOn)
                .To<T>()
                .ToList();

        public T GetById<T>(string contestId)
            => this.contestRepository
                .All()
                .Where(x => x.Id == contestId)
                .To<T>()
                .FirstOrDefault();

        public string GetContestIdByPassword(string password)
            => this.contestRepository
                .All()
                .Where(x => x.Password == password)
                .Select(x => x.Id)
                .FirstOrDefault();

        public string GetQuizIdByContestId(string contestId)
        => this.contestRepository
                .All()
                .Where(x => x.Id == contestId)
                .Select(x => x.QuizId)
                .FirstOrDefault();

        public bool IsAvailable(string password)
        {
            var findContestByPassword =
                this.contestRepository
                    .All()
                    .FirstOrDefault(x => x.Password == password);

            if (findContestByPassword == null)
            {
                return false;
            }

            var isAvailable = DateTime.UtcNow >= findContestByPassword.StartDateTime &&
                             DateTime.UtcNow <= findContestByPassword.EndDateTime;

            return isAvailable;
        }
    }
}
