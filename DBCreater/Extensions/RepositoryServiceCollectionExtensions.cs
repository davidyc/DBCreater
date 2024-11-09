using DBCreater.Models;
using DBCreater.Repositories.Implementation;
using DBCreater.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DBCreater.Extensions
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IRepository<UserProfile>, Repository<UserProfile>>();
            services.AddScoped<IRepository<Role>, Repository<Role>>();

            return services;
        }
    }
}