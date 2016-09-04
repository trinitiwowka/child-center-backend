using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericBackend.Repository.Article
{
    public class ArticleRepository : MongoRepository<DataModels.Article.Article>, IMongoRepository<DataModels.Article.Article>
    {
    }
}
