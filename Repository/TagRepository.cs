using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace Repository
{
    public class TagRepository : GenericRepository<Tag>
    {
        public TagRepository() { }

        public async Task<List<Tag>> GetAllAsync()
            => await _context.Tags.Include(t => t.NewsArticles).ToListAsync();

        public async Task<Tag> GetByIdAsync(int id)
            => await _context.Tags.Include(t => t.NewsArticles)
                                  .FirstOrDefaultAsync(t => t.TagId == id);

        public async Task CreateAsync(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tag tag)
        {
            _context.Entry(tag).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Tag tag)
        {
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasNewsArticlesAsync(int tagId)
        {
            var tag = await GetByIdAsync(tagId);
            return tag != null && tag.NewsArticles.Any();
        }
    }
}

