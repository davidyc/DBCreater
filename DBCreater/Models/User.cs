namespace DBCreater.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }

     
        public UserProfile Profile { get; set; }


        public List<Article> Articles { get; set; } = new List<Article>();

      
        public List<Role> Roles { get; set; } = new List<Role>();
    }

}
