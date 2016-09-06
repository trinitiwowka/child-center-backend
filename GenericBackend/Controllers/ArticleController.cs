using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
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
        private const string stringInfo = "base64,";

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
            if (!article.Image.StartsWith("data:image"))
            {
                return BadRequest("No picture uploaded!");
            }
            var userName = UserModel.GetUserInfo(User).Name;
            var newArticle = new Article
            {
                User = userName,
                Headline = article.Headline,
                DateOfPost = DateTime.UtcNow,
                FullText = article.FullText,
                Image = article.Image.Substring(article.Image.IndexOf(stringInfo)+stringInfo.Length),
                Summary = article.Summary
            };
            _unitOfWork.Articles.Add(newArticle);
            return Ok();
        }

        [HttpGet]
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
        }

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
    }
}