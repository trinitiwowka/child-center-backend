using GenericBackend.DataModels.Article;
using GenericBackend.Repository;
using GenericBackend.Repository.Admin;

namespace GenericBackend.UnitOfWork.Project
{
    public interface IUnitOfWork
    {
        UserRepository Users { get; }
        IMongoRepository<Article> Articles { get; }
    }

}
