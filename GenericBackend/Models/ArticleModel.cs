using System;
using System.Web;

namespace GenericBackend.Models
{
    public class ArticleModel
    {
        public string Headline;
        public string Summary;
        public string FullText;
        public DateTime DateOfPost;
        public HttpPostedFileBase Image;
        public string User;
    }
}