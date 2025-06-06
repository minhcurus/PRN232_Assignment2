using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Repository.Models;

namespace Service
{
    public class TagService
    {
        private readonly TagRepository _repo;

        public TagService()
        {
            _repo = new TagRepository();
        }

        public async Task<List<Tag>> GetAllTags()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Tag> GetTagById(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<bool> CreateTag(string name, string note)
        {
            var tags = await _repo.GetAllAsync();
            var tag = new Tag
            {
                TagId = tags.Count > 0 ? tags.Max(t => t.TagId) + 1 : 1,
                TagName = name,
                Note = note
            };

            await _repo.CreateAsync(tag);
            return true;
        }

        public async Task<bool> UpdateTag(int id, string? name, string? note)
        {
            var tag = await _repo.GetByIdAsync(id);
            if (tag == null) return false;

            if (name != null) tag.TagName = name;
            if (note != null) tag.Note = note;

            await _repo.UpdateAsync(tag);
            return true;
        }

        public async Task<bool> DeleteTag(int id)
        {
            var tag = await _repo.GetByIdAsync(id);
            if (tag == null) return false;

            if (await _repo.HasNewsArticlesAsync(id))
                return false;

            await _repo.DeleteAsync(tag);
            return true;
        }
    }
}

