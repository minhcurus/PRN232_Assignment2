using Microsoft.EntityFrameworkCore;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class NewsArticleRepository : GenericRepository<NewsArticle>
    {
        public NewsArticleRepository()
        {
        }

        public async Task<NewsArticle> GetNewsArticleById(string id)
            => await _context.NewsArticles.Include(a => a.Tags).Where(a => a.NewsArticleId.Equals(id)).FirstOrDefaultAsync();

        public async Task<List<NewsArticle>> GetNewsArticlesByCreatorId(short id)
            => await _context.NewsArticles.Include(a => a.CreatedBy).Include(a => a.Category).Include(a => a.Tags).Where(a => a.CreatedBy.AccountId.Equals(id)).ToListAsync();

        public async Task<List<NewsArticle>> GetAllNewsArticles()
            => await _context.NewsArticles.Include(a => a.CreatedBy).Include(a => a.Category).Include(a => a.Tags).Where(a => a.NewsStatus == true).ToListAsync();
    }
}
