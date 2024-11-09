using DBCreater.Models;
using Bogus;
using DBCreater.SQL;


namespace DBCreater.Generators;

public class TestDataGenerator
{
    public static List<User> GenerateUsers(int count)
    {
        var profileFaker = new Faker<UserProfile>()
            .RuleFor(p => p.Bio, f => f.Lorem.Sentence())
            .RuleFor(p => p.Website, f => f.Internet.Url());

        var userFaker = new Faker<User>()
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.CreatedAt, f => f.Date.Past(2).ToUniversalTime()) 
            .RuleFor(u => u.Profile, f => profileFaker.Generate());

        return userFaker.Generate(count);
    }

    public static List<Role> GenerateRoles()
    {
        return new List<Role>
        {
            new Role { Name = "Admin" },
            new Role { Name = "User" },
            new Role { Name = "Guest" }
        };
    }

    public static List<Article> GenerateArticles(User user, int count)
    {
        var articleFaker = new Faker<Article>()
            .RuleFor(a => a.Title, f => f.Lorem.Sentence())
            .RuleFor(a => a.Content, f => f.Lorem.Paragraphs(1, 3))
            .RuleFor(a => a.User, user); 

        return articleFaker.Generate(count);
    }

    public static void SeedUserWithArticlesDatabase(TestDBContext context, int countUser, int countRandomArcticle )
    {
        var users = TestDataGenerator.GenerateUsers(countUser);        
        context.Users.AddRange(users);
        var random = new Random();      

        foreach (var user in users)
        {
            var randomNumber = random.Next(0, countRandomArcticle);
            var articles = TestDataGenerator.GenerateArticles(user, randomNumber);
            context.Articles.AddRange(articles);
        }

        context.SaveChanges();
    }
}
