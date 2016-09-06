using System;
using GenericBackend.Core;

namespace GenericBackend.DataModels.Article
{
    public class Article : MongoEntityBase
    {
        public string Headline;
        public string Summary;
        public string FullText;
        public DateTime DateOfPost;
        public string Image;
        public string User;
    }
}
