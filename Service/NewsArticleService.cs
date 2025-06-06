using Repository;
using Repository.Models;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class NewsArticleService
    {
        private readonly NewsArticleRepository _repo;
        private readonly AccountRepository _accountRepository;

        public NewsArticleService()
        {
            _repo = new NewsArticleRepository();
            _accountRepository = new AccountRepository();
        }

        public async Task<List<NewsArticle>> GetNewsArticles()
        {
            return await _repo.GetAllNewsArticles();
        }

        public async Task<NewsArticle> GetNewsArticleById(string id)
        {
            return await _repo.GetNewsArticleById(id);
        }

        public async Task<List<NewsArticle>> GetNewsArticleByAccountId(short id)
        {
            return await _repo.GetNewsArticlesByCreatorId(id);
        }

        public async Task<bool> CreateNewsArticle(CreateNewsArticleDTO newsArticle)
        {

            var l = await _repo.GetAllAsync();

            await _repo.CreateAsync(new NewsArticle
            {
                NewsArticleId = (l.Count + 1).ToString(),
                CategoryId = newsArticle.CategoryId,
                CreatedById = newsArticle.CreatedById,
                CreatedDate = DateTime.Now,
                Headline = newsArticle.Headline,
                NewsContent = newsArticle.NewsContent,
                NewsSource = newsArticle.NewsSource,
                NewsStatus = newsArticle.NewsStatus,
                NewsTitle = newsArticle.NewsTitle,
            });
            return true;
        }

        public async Task<bool> RemoveNewsArticle(string id)
        {
            var newsArticle = await _repo.GetNewsArticleById(id);

            if (newsArticle == null)
            {
                return false;
            }

            await _repo.RemoveAsync(newsArticle);
            return true;
        }

        public async Task<bool> UpdateNewsArticle(string id,CreateNewsArticleDTO newsArticle)
        {
            var oldNewsArticle = await _repo.GetNewsArticleById(id);

            if (oldNewsArticle == null)
            {
                return false;
            }

            oldNewsArticle.CategoryId = newsArticle.CategoryId;
            oldNewsArticle.Headline = newsArticle.Headline;
            oldNewsArticle.NewsContent = newsArticle.NewsContent;
            oldNewsArticle.NewsSource = newsArticle.NewsSource;
            oldNewsArticle.NewsStatus = newsArticle.NewsStatus;
            oldNewsArticle.NewsTitle = newsArticle.NewsTitle;
            oldNewsArticle.UpdatedById = newsArticle.UpdatedById;
            oldNewsArticle.ModifiedDate = DateTime.Now;

            await _repo.UpdateAsync(oldNewsArticle);
            return true;
        }
    }
}
