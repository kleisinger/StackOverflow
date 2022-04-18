namespace StackOverload.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public bool? Voted { get; set; }

        public ApplicationUser User { get; set; }
        public int UserId { get; set; }

        public Question? Question { get; set; }
        public int? QuestionId { get; set; }

        public Answer? Answer { get; set; }
        public int? AnswerId { get; set; }


        public Vote(ApplicationUser user, Question question)
        {
            User = user;
            Question = question;
        }

        public Vote(ApplicationUser user, Answer answer)
        {
            User = user;
            Answer = answer;
        }

        public Vote()
        {

        }

    }
}
