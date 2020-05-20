namespace QuizSystem.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

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
        private readonly IDeletableEntityRepository<UserResult> userResultRepository;

        public QuizzesService(
            IDeletableEntityRepository<Quiz> quizRepository,
            IDeletableEntityRepository<Question> questionRepository,
            IDeletableEntityRepository<Answer> answerRepository)
        {
            this.quizRepository = quizRepository;
            this.questionRepository = questionRepository;
            this.answerRepository = answerRepository;
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

        public async Task SubmitAsync(ContestViewModel model, string userId)
        {
            var selectedAnswers = model.Quiz
                .Questions
                .SelectMany(x => x.Answers)
                .Where(x => x.IsChecked)
                .Select(x => x.Id)
                .ToArray();

            var userResult = new UserResult
            {
                ApplicationUserId = userId,
                ContestId = model.Id,
                FinishedOn = DateTime.UtcNow,
            };

            foreach (var question in model.Quiz.Questions)
            {
                var userQuestion = new UserQuestion
                {
                    QuestionId = question.Id,
                };

                foreach (var answer in question.Answers)
                {
                    userQuestion.UserAnswers.Add(new UserAnswer
                    {
                        SelectedId = answer.Id,
                        Text = answer.Text,
                    });
                }

                userResult.UserQuestions.Add(userQuestion);
            }

            await this.userResultRepository.AddAsync(userResult);
            await this.userResultRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(string quizId)
        {
            var quiz = this.quizRepository
                .All()
                .FirstOrDefault(x => x.Id == quizId);

            this.quizRepository.Delete(quiz);

            await this.quizRepository.SaveChangesAsync();
        }

        public async Task EditAsync(EditQuizInputModel inputModel)
        {
            var quiz = this.quizRepository
                .All()
                .FirstOrDefault(x => x.Id == inputModel.Id);

            quiz.Name = inputModel.Name;

            await this.quizRepository.SaveChangesAsync();
        }
    }
}
