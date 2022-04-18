using Microsoft.AspNetCore.Identity;

namespace StackOverload.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Reputation { get; set; } = 0;

        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }


        public ApplicationUser()
        {
            Reputation = 0;
            Questions = new HashSet<Question>();
            Answers = new HashSet<Answer>();
            Comments = new HashSet<Comment>();
            Votes = new HashSet<Vote>();
        }


    }
}
