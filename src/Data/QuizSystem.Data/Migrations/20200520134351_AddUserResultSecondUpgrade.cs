using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuizSystem.Data.Migrations
{
    public partial class AddUserResultSecondUpgrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Questions_QuestionId",
                table: "UserAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_UserResults_UserResultId",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_QuestionId",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_UserResultId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "UserContests");

            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "IsFreeAnswer",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "Selected",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "UserResultId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "IsFreeAnswer",
                table: "Answers");

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedOn",
                table: "UserResults",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SelectedId",
                table: "UserAnswers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserQuestionId",
                table: "UserAnswers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserQuestions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    UserResultId = table.Column<string>(nullable: true),
                    QuestionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserQuestions_UserResults_UserResultId",
                        column: x => x.UserResultId,
                        principalTable: "UserResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_UserQuestionId",
                table: "UserAnswers",
                column: "UserQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestions_IsDeleted",
                table: "UserQuestions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestions_QuestionId",
                table: "UserQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestions_UserResultId",
                table: "UserQuestions",
                column: "UserResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_UserQuestions_UserQuestionId",
                table: "UserAnswers",
                column: "UserQuestionId",
                principalTable: "UserQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_UserQuestions_UserQuestionId",
                table: "UserAnswers");

            migrationBuilder.DropTable(
                name: "UserQuestions");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_UserQuestionId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "FinishedOn",
                table: "UserResults");

            migrationBuilder.DropColumn(
                name: "SelectedId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "UserQuestionId",
                table: "UserAnswers");

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "UserContests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "UserAnswers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFreeAnswer",
                table: "UserAnswers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "QuestionId",
                table: "UserAnswers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Selected",
                table: "UserAnswers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserResultId",
                table: "UserAnswers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFreeAnswer",
                table: "Answers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_QuestionId",
                table: "UserAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_UserResultId",
                table: "UserAnswers",
                column: "UserResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Questions_QuestionId",
                table: "UserAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_UserResults_UserResultId",
                table: "UserAnswers",
                column: "UserResultId",
                principalTable: "UserResults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
