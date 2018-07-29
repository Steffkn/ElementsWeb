namespace Elements.Models.Forum
{
    public class UsersReplies
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int ReplyId { get; set; }

        public Reply Reply { get; set; }
    }
}
