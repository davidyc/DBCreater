namespace DBCreater.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

     
        public int UserId { get; set; }
        public User User { get; set; }
    }

}
