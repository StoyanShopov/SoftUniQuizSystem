namespace QuizHut.Services.Questions
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using OfficeOpenXml;

    using QuizHut.Data.Common.Repositories;
    using QuizHut.Data.Models;
    using QuizHut.Services.Mapping;

    public class QuestionsService : IQuestionsService
    {
        private readonly IDeletableEntityRepository<Question> questionRepository;
        private readonly IDeletableEntityRepository<Quiz> quizRepository;
        private readonly IDeletableEntityRepository<Answer> answerRepository;

        public QuestionsService(
            IDeletableEntityRepository<Question> questionRepository,
            IDeletableEntityRepository<Quiz> quizRepository,
            IDeletableEntityRepository<Answer> answerRepository)
        {
            this.questionRepository = questionRepository;
            this.quizRepository = quizRepository;
            this.answerRepository = answerRepository;
        }

        public async Task<string> CreateQuestionAsync(string quizId, string questionText)
        {
            var quiz = await this.quizRepository.AllAsNoTracking().Select(x => new
            {
                x.Id,
                Questions = x.Questions.Count,
            }).FirstOrDefaultAsync(x => x.Id == quizId);

            var question = new Question
            {
                Number = quiz.Questions + 1,
                Text = questionText,
                QuizId = quizId,
            };

            await this.questionRepository.AddAsync(question);
            await this.questionRepository.SaveChangesAsync();

            return question.Id;
        }

        public async Task<string> ImportQuestionsAsync(string quizId, IFormFile formFile)
        {
            await this.DeleteAllQuestionsInQuiz(quizId);

            var quiz = await this.quizRepository.All()
                .FirstOrDefaultAsync(q => q.Id == quizId);

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
                        Number = quiz.Questions.Count + 1,
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
                            IsRightAnswer = isRightAnswer,
                            Question = question,
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

        public async Task DeleteQuestionByIdAsync(string id)
        {
            var question = await this.questionRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            this.questionRepository.Delete(question);
            await this.questionRepository.SaveChangesAsync();
            await this.UpdateAllQuestionsInQuizNumeration(question.QuizId);
        }

        public async Task UpdateAllQuestionsInQuizNumeration(string quizId)
        {
            var count = 0;

            var questions = this.questionRepository
              .AllAsNoTracking()
              .Where(x => x.QuizId == quizId)
              .OrderBy(x => x.Number);

            foreach (var question in questions)
            {
                question.Number = ++count;
                this.questionRepository.Update(question);
            }

            await this.questionRepository.SaveChangesAsync();
        }

        public async Task Update(string id, string text)
        {
            var question = await this.questionRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            question.Text = text;
            this.questionRepository.Update(question);
            await this.questionRepository.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync<T>(string id)
        => await this.questionRepository.AllAsNoTracking()
            .Where(x => x.Id == id)
            .To<T>()
            .FirstOrDefaultAsync();

        public async Task<IList<T>> GetAllByQuizIdAsync<T>(string id)
        => await this.questionRepository.AllAsNoTracking()
            .Where(x => x.QuizId == id)
            .OrderBy(x => x.Number)
            .To<T>()
            .ToListAsync();

        public int GetAllByQuizIdCount(string id)
        => this.questionRepository.AllAsNoTracking().Count(x => x.QuizId == id);

        public async Task<T> GetQuestionByQuizIdAndNumberAsync<T>(string quizId, int number)
        => await this.questionRepository.AllAsNoTracking()
            .Where(x => x.QuizId == quizId && x.Number == number)
            .To<T>()
            .FirstOrDefaultAsync();

        private async Task DeleteAllQuestionsInQuiz(string quizId)
        {
            var questions = this.quizRepository
                .AllAsNoTracking()
                .Where(q => q.Id == quizId)
                .Select(q => q.Questions.Select(qq => qq.Id).ToList())
                .ToList()
                .FirstOrDefault();

            foreach (var questionId in questions)
            {
                await this.DeleteQuestionByIdAsync(questionId);
            }
        }
    }
}
