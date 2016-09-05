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

        public ArticleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public IHttpActionResult AddArticle(Article article)
        //public IHttpActionResult AddArticle(Article article, HttpPostedFileBase photoImg)
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
                //Image = GetPhoto(photoImg),
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

        #region TestPart
        /*
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {

            return Ok(await GetAllArticles());
        }

        private Task<List<ArticleModel>> GetAllArticles()
        {
            var sPath = "";
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/news-image-1.png");
            var newImage = Image.FromFile(sPath);
            var sPath2 = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/news-image-2.png");
            var newImage2= Image.FromFile(sPath2);
            var article1 = new ArticleModel
            {
                Headline = "Test1",
                DateOfPost = DateTime.UtcNow,
                Image = TakePhoto(newImage),
                FullText = "Full Text1",
                Summary = "Summary1"
            };
            var article2 = new ArticleModel
            {
                Headline = "Test2",
                DateOfPost = DateTime.UtcNow,
                Image = TakePhoto(newImage2),
                FullText = "Full Text2",
                Summary = "Summary2"
            };
            var newList = new List<ArticleModel> { article1, article2 };
            var query = newList.AsQueryable();
            /*_unitOfWork.Articles.Add( new Article
            {
                Headline = "Test1",
                DateOfPost = DateTime.UtcNow,
                FullText = "Full Text1",
                Summary = "Summary1"
            });
            return Task.Factory.StartNew(() => query.ToList());
    }*/

    #endregion

    private byte[] GetPhoto(HttpPostedFileBase file)
        {
            byte[] buffer = null;

            if (file != null && file.ContentLength <= 1 * 1024 * 1024)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    buffer = ms.GetBuffer();
                }
            }

            return buffer;
        }
        
        private string TakePhoto(Image imageIn)
        {
            byte[] imageInByteArray = ImageToByteArray(imageIn);
            return Convert.ToBase64String(imageInByteArray);
        }

        private byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}