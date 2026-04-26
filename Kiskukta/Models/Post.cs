namespace Kiskukta.Models
{
    public class Post
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string Comment { get; set; }
        public PostStatus Status { get; set; }
    }
}