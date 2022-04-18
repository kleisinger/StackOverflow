namespace StackOverload.Models
{
    public class TagQuestion
    {
        public int Id { get; set; }

        public int TagId { get; set; }
        public int QuestionId { get; set; }

        public Tag Tag { get; set; }

        public Question Question { get; set; }

        public TagQuestion(Tag tag, Question question)
        {
            Tag = tag;
            Question = question;
        }

        public TagQuestion()
        {

        }
    }
}
