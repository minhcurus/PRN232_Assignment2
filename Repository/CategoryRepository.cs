using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Models;

namespace Repository
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository() { }

        public async Task<List<Category>> GetAllAsync()
            => await _context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.NewsArticles)
                .ToListAsync();

        public async Task<Category> GetByIdAsync(short id)
            => await _context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.NewsArticles)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

        public async Task CreateAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasRelatedNewsAsync(short id)
        {
            return await _context.NewsArticles.AnyAsync(n => n.CategoryId == id);
        }
    }
}

