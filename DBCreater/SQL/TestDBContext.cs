using DBCreater.Generators;
using DBCreater.Models;
using Microsoft.EntityFrameworkCore;

namespace DBCreater.SQL;

public class TestDBContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Role> Roles { get; set; }

    public TestDBContext(DbContextOptions<TestDBContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {        
        modelBuilder.Entity<User>()
            .HasOne(u => u.Profile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfile>(p => p.UserId);

      
        modelBuilder.Entity<User>()
            .HasMany(u => u.Articles)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId);

      
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<Dictionary<string, object>>(
                "UserRole",
                j => j.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                j => j.HasOne<User>().WithMany().HasForeignKey("UserId"));
    }

    public static TestDBContext Create(string connectionString, DatabaseType dbType)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TestDBContext>();

        switch (dbType)
        {
            case DatabaseType.SqlServer:
                optionsBuilder.UseSqlServer(connectionString);
                break;
            case DatabaseType.PostgreSql:
                optionsBuilder.UseNpgsql(connectionString);
                break;
            default:
                throw new NotSupportedException($"Database type '{dbType}' is not supported.");
        }

        return new TestDBContext(optionsBuilder.Options);
    }

    public void InitializeDatabase(int countUser, int countArcticle)
    {
        Database.EnsureCreated();
        SeedData();
        TestDataGenerator.SeedUserWithArticlesDatabase(this, countUser, countArcticle);
    }

    private void SeedData()
    {
        if (!Users.Any())
        {
          
            var adminRole = new Role { Name = "Admin" };
            var userRole = new Role { Name = "User" };
            Roles.AddRange(adminRole, userRole);
            SaveChanges();

            var user1 = new User
            {
                Username = "johndoe",
                Email = "johndoe@example.com",
                CreatedAt = DateTime.UtcNow,
                Profile = new UserProfile { Bio = "Developer", Website = "https://johndoe.dev" },
                Roles = new List<Role> { adminRole }
            };

            var user2 = new User
            {
                Username = "janedoe",
                Email = "janedoe@example.com",
                CreatedAt = DateTime.UtcNow,
                Profile = new UserProfile { Bio = "Designer", Website = "https://janedoe.design" },
                Roles = new List<Role> { userRole }
            };

            Users.AddRange(user1, user2);
            SaveChanges();

           
            Articles.AddRange(
                new Article { Title = "Intro to C#", Content = "Content for C#", UserId = user1.UserId },
                new Article { Title = "Design Principles", Content = "Content for Design", UserId = user2.UserId }
            );

            SaveChanges();
        }
    }
}
