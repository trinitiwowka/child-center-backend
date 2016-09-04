using GenericBackend.DataModels.Article;
using GenericBackend.Repository;
using GenericBackend.Repository.Admin;
using GenericBackend.Repository.Article;

namespace GenericBackend.UnitOfWork.Project
{
    public class UnitOfWork : IUnitOfWork
    {
        private UserRepository _users;
        private IMongoRepository<Article> _articles;

        public UserRepository Users => _users ?? (_users = new UserRepository());
        public IMongoRepository<Article> Articles => _articles ?? (_articles = new ArticleRepository());
        
    }
}