namespace QuizSystem.Web.ViewModels.Contests.InputModel
{
    using System.ComponentModel.DataAnnotations;

    public class StarContestInputModel
    {
        private const int MaximumLength = 15;
        private const int MinimumLength = 2;
        private const string ErrorMessage = "Invalid password!";

        [Required]
        [StringLength(MaximumLength, MinimumLength = MinimumLength, ErrorMessage = ErrorMessage)]
        public string Password { get; set; }
    }
}
