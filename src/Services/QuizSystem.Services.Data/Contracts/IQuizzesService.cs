namespace QuizSystem.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using QuizSystem.Web.ViewModels.Administration.Quizzes.InputModels;
    using QuizSystem.Web.ViewModels.Quizzes.InputModels;

    public interface IQuizzesService
    {
        Task<string> CreateAsync(CreateQuizInputModel inputModel);

        IEnumerable<T> GetAll<T>();

        T GetById<T>(string quizId);

        Task<string> ImportQuestionsAsync(string quizId, IFormFile formFile);

        Task<int> SubmitAsync(QuizSubmitInputModel model);
    }
}
