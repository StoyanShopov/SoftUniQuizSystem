namespace QuizSystem.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using QuizSystem.Web.ViewModels.Administration.Questions.InputModels;

    public interface IQuestionsService
    {
        Task<string> CreateAsync(CreateQuestionInputModel model);

        Task<string> EditAsync(EditQuestionInputModel model);

        T GetById<T>(string id);
    }
}
