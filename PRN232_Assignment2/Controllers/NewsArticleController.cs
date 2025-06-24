using CoreApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service;
using Service.DTO;

namespace Assignment2_PRN232.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsArticleController : BaseController
    {
        private readonly NewsArticleService _service;

        public NewsArticleController(NewsArticleService service)
        {
            _service = service;
        }

        [HttpGet("GetNewsArticles")]
        public async Task<IActionResult> GetNewsArticles()
        {
            return CustomResult(await _service.GetNewsArticles());
        }

        [HttpGet("GetNewsArticlesByAccountId/{id}")]
        public async Task<IActionResult> GetNewsArticlesByAccountId(short id)
        {
            var newArticle = await _service.GetNewsArticleByAccountId(id);
            if (newArticle.IsNullOrEmpty())
            {
                return CustomResult("No article of this user.");
            }
            return CustomResult(newArticle);
        }

        [HttpGet("GetNewsArticleById/{id}")]
        public async Task<IActionResult> GetNewsArticleById(string id)
        {
            var newArticle = await _service.GetNewsArticleById(id);
            if (newArticle == null)
            {
                return CustomResult("No article found.");
            }
            return CustomResult(newArticle);
        }

        [Authorize(Roles = "1")]
        [HttpPost("CreateNewsArticle")]
        public async Task<IActionResult> CreateNewsArticle(CreateNewsArticleDTO newsArticle)
        {
            if (await _service.CreateNewsArticle(newsArticle)) return CustomResult("Create news article successfully.");
            return CustomResult("Failed.");
        }

        [Authorize(Roles = "1")]
        [HttpDelete("DeleteNewsArticle")]
        public async Task<IActionResult> DeleteNewsArticle(string id)
        {
            if (await _service.RemoveNewsArticle(id)) return CustomResult("Successfully");
            return CustomResult("Failed");
        }

        [Authorize(Roles = "1")]
        [HttpPut("UpdateNewsArticle/{id}")]
        public async Task<IActionResult> UpdateNewsArticle(string id, CreateNewsArticleDTO newsArticle)
        {
            if (await _service.UpdateNewsArticle(id, newsArticle)) return CustomResult("Successfully");
            return CustomResult("Failed");
        }

        [Authorize(Roles = "0")]
        [HttpGet("GetNewsArticlesReport")]
        public async Task<IActionResult> GetNewsArticlesReport(DateTime startDate, DateTime endDate)
        {
            var report = await _service.GetNewsArticleStatics(startDate, endDate);
            return CustomResult(report);
        }
    }
}
