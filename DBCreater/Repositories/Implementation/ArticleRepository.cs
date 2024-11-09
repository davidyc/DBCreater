using DBCreater.Models;
using DBCreater.Repositories.Interfaces;
using DBCreater.SQL;

namespace DBCreater.Repositories.Implementation
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        public ArticleRepository(TestDBContext context) : base(context)
        {
        }        
    }
}
