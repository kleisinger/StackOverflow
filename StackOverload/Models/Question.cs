namespace StackOverload.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime PostDate { get; set; }
        //public int? Votes { get; set; }
        public bool IsAnswered { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public ICollection<TagQuestion> TagQuestions { get; set; }
        public ICollection<Vote> Votes { get; set; }

        public Question(string title, string body, ApplicationUser user)
        {
            Title = title;
            Body = body;
            PostDate = DateTime.Now;
            //Votes = 0;
            IsAnswered = false;
            User = user;
            Answers = new List<Answer>();
            Comments = new List<Comment>();
            TagQuestions = new List<TagQuestion>();
            Votes = new List<Vote>();
        }
        public Question()
        {

        }
    }
}
