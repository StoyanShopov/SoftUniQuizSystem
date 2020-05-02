namespace QuizSystem.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Conventions;
    using OfficeOpenXml;
    using QuizSystem.Data.Common.Repositories;
    using QuizSystem.Data.Models;
    using QuizSystem.Services.Mapping;
    using QuizSystem.Web.ViewModels.Administration.Quizzes.InputModels;
    using QuizSystem.Web.ViewModels.Quizzes.ViewModels;

    public class QuizzesService : IQuizzesService
    {
        private readonly IDeletableEntityRepository<Quiz> quizRepository;
        private readonly IDeletableEntityRepository<Question> questionRepository;
        private readonly IDeletableEntityRepository<Answer> answerRepository;
        private readonly IDeletableEntityRepository<UserContest> userContests;

        public QuizzesService(
            IDeletableEntityRepository<Quiz> quizRepository,
            IDeletableEntityRepository<Question> questionRepository,
            IDeletableEntityRepository<Answer> answerRepository,
            IDeletableEntityRepository<UserContest> userContests)
        {
            this.quizRepository = quizRepository;
            this.questionRepository = questionRepository;
            this.answerRepository = answerRepository;
            this.userContests = userContests;
        }

        public async Task<string> CreateAsync(CreateQuizInputModel inputModel)
        {
            var quiz = new Quiz
            {
                Name = inputModel.Name,
            };

            await this.quizRepository.AddAsync(quiz);
            await this.quizRepository.SaveChangesAsync();

            return quiz.Id;
        }

        public IEnumerable<T> GetAll<T>()
            => this.quizRepository.All()
                .OrderByDescending(x => x.CreatedOn)
                .To<T>()
                .ToList();

        public T GetById<T>(string quizId)
            => this.quizRepository.All()
                .Where(x => x.Id == quizId)
                .To<T>()
                .FirstOrDefault();

        public async Task<string> ImportQuestionsAsync(string quizId, IFormFile formFile)
        {
            var quiz = this.quizRepository.All()
                .FirstOrDefault(q => q.Id == quizId);

            await using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using var package = new ExcelPackage(stream);
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                var rows = worksheet.Dimension.Rows;
                var cols = worksheet.Dimension.Columns;

                for (int row = 2; row <= rows; row++)
                {
                    var questionText = worksheet.Cells[row, 1].Text;

                    var question = new Question
                    {
                        QuizId = quiz.Id,
                        Text = questionText,
                    };

                    await this.questionRepository.AddAsync(question);

                    for (int col = 2; col <= cols; col++)
                    {
                        var answerText = worksheet.Cells[row, col].Text;
                        var isRightAnswer = worksheet.Cells[row, col].Style.Fill.BackgroundColor.Rgb != null;

                        var answer = new Answer
                        {
                            Text = answerText,
                            IsCorrect = isRightAnswer,
                            QuestionId = question.Id,
                        };

                        await this.answerRepository.AddAsync(answer);
                    }
                }

                package.Dispose();
            }

            await this.questionRepository.SaveChangesAsync();
            await this.answerRepository.SaveChangesAsync();

            return quizId;
        }

        public async Task<ResultQuizViewModel> SubmitAsync(ContestViewModel model, string userId)
        {
            var correctAnswersCollection = this.quizRepository
                .All()
                .Include(x => x.Questions)
                .ThenInclude(x => x.Answers)
                .Where(x => x.Id == model.Quiz.Id)
                .SelectMany(x => x.Questions)
                .SelectMany(x => x.Answers)
                .Where(a => a.IsCorrect)
                .Select(t => t.Id)
                .ToList();

            var selectedAnswers = model.Quiz.Questions
                .SelectMany(x => x.Answers)
                .Where(x => x.IsChecked)
                .Select(x => x.Id)
                .ToList();

            var contest = this.userContests
                .All()
                .FirstOrDefault(x => x.ContestId == model.Id && x.ApplicationUserId == userId);

            int totalPoints = correctAnswersCollection.Count;
            int resultPoints = correctAnswersCollection.Intersect(selectedAnswers).Count();
            int wrongAnswers = totalPoints - resultPoints;

            contest.Points = resultPoints;

            await this.userContests.SaveChangesAsync();

            var result = new ResultQuizViewModel
            {
                TotalPoints = totalPoints,
                CorrectAnswers = resultPoints,
                WrongAnswers = wrongAnswers,
            };

            return result;
        }
    }
}
