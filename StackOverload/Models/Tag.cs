namespace StackOverload.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }


        //public int? TagQuestionId { get; set; }
        public ICollection<TagQuestion> TagQuestions { get; set; }


    }
}
