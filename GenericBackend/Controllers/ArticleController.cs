using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using GenericBackend.DataModels.Article;
using GenericBackend.Models;
using GenericBackend.UnitOfWork.Project;
using System.Threading.Tasks;

namespace GenericBackend.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/article")]
    public class ArticleController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArticleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public IHttpActionResult AddArticle(Article article)
        {
            if (article == null)
                return BadRequest("Article can't be null");
            if (string.IsNullOrEmpty(article.Headline))
                return BadRequest("Article headline can't be empty");
            var userName = UserModel.GetUserInfo(User).Name;
            var newArticle = new Article
            {
                User = userName,
                Headline = article.Headline,
                DateOfPost = DateTime.UtcNow,
                FullText = article.FullText,
                Image = article.Image,
                Summary = article.Summary
            };
            _unitOfWork.Articles.Add(newArticle);
            return Ok();
        }

        /*        [HttpGet]
                public async Task<IHttpActionResult> Get()
                {

                    return Ok(await GetAllArticles());
                }

                private Task<List<Article>> GetAllArticles()
                {
                    var user = UserModel.GetUserInfo(User);
                    var query = _unitOfWork.Articles.AsQueryable();
                    if (!user.IsSuperUser)
                        query = _unitOfWork.Articles.Where(x => x.User == user.Name);

                    return Task.Factory.StartNew(() => query.ToList());
                }*/

        [HttpGet]
        [Route("number/{id}")]
        public IHttpActionResult GetArticle(string id)
        {
            try
            {
                return Ok(_unitOfWork.Articles.FirstOrDefault((p) => p.Id == id));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        #region TestPart

        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {

            return Ok(await GetAllArticles());
        }

        private Task<List<Article>> GetAllArticles()
        {
            var article1 = new Article
            {
                Headline = "Test1",
                DateOfPost = DateTime.UtcNow,
                FullText = "Full Text1",
                Summary = "Summary1"
            };
            var article2 = new Article
            {
                Headline = "Test2",
                DateOfPost = DateTime.UtcNow,
                FullText = "Full Text2",
                Summary = "Summary2"
            };
            var newList = new List<Article> { article1, article2 };
            var query = newList.AsQueryable();
            return Task.Factory.StartNew(() => query.ToList());
        }

        #endregion
    }
}