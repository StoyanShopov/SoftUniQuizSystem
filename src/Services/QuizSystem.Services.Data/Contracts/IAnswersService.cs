namespace QuizSystem.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using QuizSystem.Web.ViewModels.Administration.Answers.InputModels;

    public interface IAnswersService
    {
        Task<string> CreateAsync(CreateAnswerInputModel inputModel);

        Task<string> EditAsync();

        T GetById<T>(string id);
    }
}
