namespace StackOverload.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime PostDate { get; set; }
        //public int? Votes { get; set; } = 0;
        public bool IsCorrect { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Vote> Votes { get; set; }


        public Answer(string body, 
            ApplicationUser user, Question question)
        {
            Body = body;
            PostDate = DateTime.Now;
            //Votes = 0;
            IsCorrect = false;
            User = user;
            Question = question;
            Comments = new List<Comment>();
            Votes = new List<Vote>();
        }

        public Answer()
        {

        }
    }
}
