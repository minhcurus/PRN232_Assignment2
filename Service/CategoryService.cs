using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Repository.Models;

namespace Service
{
    public class CategoryService
    {
        private readonly CategoryRepository _repo;

        public CategoryService()
        {
            _repo = new CategoryRepository();
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Category> GetCategoryById(short id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<bool> CreateCategory(string name, string description, short? parentId, bool? isActive)
        {
            var categories = await _repo.GetAllAsync();
            var newCategory = new Category
            {
                CategoryId = (short)(categories.Count + 1),
                CategoryName = name,
                CategoryDesciption = description,
                ParentCategoryId = parentId,
                IsActive = isActive ?? true
            };
            await _repo.CreateAsync(newCategory);
            return true;
        }

        public async Task<bool> UpdateCategory(short id, string? name, string? description, short? parentId, bool? isActive)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) return false;

            if (name != null) category.CategoryName = name;
            if (description != null) category.CategoryDesciption = description;
            if (parentId != null) category.ParentCategoryId = parentId;
            if (isActive != null) category.IsActive = isActive;

            await _repo.UpdateAsync(category);
            return true;
        }

        public async Task<bool> DeleteCategory(short id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) return false;

            if (await _repo.HasRelatedNewsAsync(id))
                return false;

            await _repo.DeleteAsync(category);
            return true;
        }
    }
}
