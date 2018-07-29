namespace Elements.Models.Forum
{
    public class UsersTopics
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int TopicId { get; set; }

        public Topic Topic { get; set; }
    }
}
