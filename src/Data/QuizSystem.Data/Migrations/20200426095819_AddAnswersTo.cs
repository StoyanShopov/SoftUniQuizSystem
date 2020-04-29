namespace QuizSystem.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddAnswersTo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFreeAnswer",
                table: "Answers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "QuizId",
                table: "Answers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuizId",
                table: "Answers",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Quizzes_QuizId",
                table: "Answers",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Quizzes_QuizId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_QuizId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "IsFreeAnswer",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "Answers");
        }
    }
}
