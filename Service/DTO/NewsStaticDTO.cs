using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class NewsStaticDTO
    {
        public int numberOfNewsArticle {  get; set; }

        public List<NewsArticle>? newsArticles { get; set; }
    }
}
