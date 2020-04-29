namespace QuizSystem.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using QuizSystem.Data.Common.Repositories;
    using QuizSystem.Data.Models;
    using QuizSystem.Services.Data.Contracts;
    using QuizSystem.Web.ViewModels.Administration.Answers.InputModels;

    public class AnswersService : IAnswersService
    {
        private readonly IDeletableEntityRepository<Answer> answersRepository;

        public AnswersService(IDeletableEntityRepository<Answer> answersRepository)
        {
            this.answersRepository = answersRepository;
        }

        public async Task<string> CreateAsync(CreateAnswerInputModel inputModel)
        {
            var answer = new Answer
            {
                Text = inputModel.Text,
                IsCorrect = inputModel.IsCorrect,
                IsFreeAnswer = inputModel.IsFreeAnswer,
                QuestionId = inputModel.QuestionId,
            };

            await this.answersRepository.AddAsync(answer);
            await this.answersRepository.SaveChangesAsync();

            return answer.Id;
        }

        public Task<string> EditAsync()
        {
            throw new NotImplementedException();
        }

        public T GetById<T>(string id)
        {
            throw new NotImplementedException();
        }
    }
}
