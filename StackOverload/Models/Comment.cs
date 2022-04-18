namespace StackOverload.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime PostDate { get; set; }


        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int? QuestionId { get; set; }
        public Question? Question { get; set; }

        public int? AnswerId { get; set; }
        public Answer? Answer { get; set; }

        public Comment(string body, ApplicationUser user, Question question)
        {
            Body = body;
            User = user;
            Question = question;
        }

        public Comment(string body, ApplicationUser user, Answer answer)
        {
            Body = body;
            User = user;
            Answer = answer;
        }

        public Comment()
        {

        }
    }
}
