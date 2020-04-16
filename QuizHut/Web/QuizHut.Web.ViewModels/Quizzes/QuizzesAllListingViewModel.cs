﻿namespace QuizHut.Web.ViewModels.Quizzes
{
    using System.Collections.Generic;

    public class QuizzesAllListingViewModel
    {
        public QuizzesAllListingViewModel()
        {
            this.Quizzes = new List<QuizListViewModel>();
        }

        public IEnumerable<QuizListViewModel> Quizzes { get; set; }

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public int NextPage
        {
            get
            {
                if (this.CurrentPage >= this.PagesCount)
                {
                    return 1;
                }

                return this.CurrentPage + 1;
            }
        }

        public int PreviousPage
        {
            get
            {
                if (this.CurrentPage <= 1)
                {
                    return this.PagesCount;
                }

                return this.CurrentPage - 1;
            }
        }
    }
}
