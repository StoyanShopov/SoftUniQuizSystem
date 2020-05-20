namespace QuizSystem.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        public int TotalContests
            => this.contestRepository.AllAsNoTracking().Count();

        public async Task AssignUserToContestAsync(string userId, string contestId)
        {
            var userContest = this.userContests
                .All()
                .FirstOrDefault(x => x.ApplicationUserId == userId && x.ContestId == contestId);

            if (userContest != null)
            {
                return;
            }

            userContest = new UserContest
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

        public async Task EditAsync(EditContestInputModel model)
        {
            var contest = this.contestRepository
                .All()
                .FirstOrDefault(x => x.Id == model.Id);

            contest.QuizId = model.QuizId;
            contest.StartDateTime = model.StartDateTime;
            contest.EndDateTime = model.EndDateTime;
            contest.Name = model.Name;
            contest.Password = model.Password;

            await this.contestRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>(int skip, int take)
        {
            var contests = this.contestRepository
                .All()
                .OrderByDescending(x => x.CreatedOn)
                .To<T>()
                .ToList();

            if (contests.Count < take)
            {
                return contests;
            }

            return contests.Skip(skip * (take - 1))
                .Take(skip);
        }

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

        public async Task DeleteAsync(string contestId)
        {
            var contest = this.contestRepository
                .All()
                .FirstOrDefault(x => x.Id == contestId);

            this.contestRepository.Delete(contest);

            await this.contestRepository.SaveChangesAsync();
        }

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
