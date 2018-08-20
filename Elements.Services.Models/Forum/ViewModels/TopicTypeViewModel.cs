namespace Elements.Services.Models.Forum.ViewModels
{
    public class TopicTypeViewModel
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
