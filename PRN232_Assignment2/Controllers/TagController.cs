using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Service;

namespace Assignment2_PRN232.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly TagService _service;

        public TagController()
        {
            _service = new TagService();
        }

        [HttpGet("GetAllTags")]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _service.GetAllTags();
            return Ok(tags);
        }

        [HttpGet("GetTagById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var tag = await _service.GetTagById(id);
            if (tag == null) return NotFound();
            return Ok(tag);
        }

        [HttpPost("CreateTag")]
        public async Task<IActionResult> Create([FromBody] Tag tag)
        {
            var result = await _service.CreateTag(tag.TagName, tag.Note);
            if (!result) return BadRequest("Failed to create tag.");
            return Ok("Tag created.");
        }

        [HttpPut("UpdateTag/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Tag tag)
        {
            var result = await _service.UpdateTag(id, tag.TagName, tag.Note);
            if (!result) return BadRequest("Update failed.");
            return Ok("Tag updated.");
        }

        [HttpDelete("DeleteTag/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteTag(id);
            if (!result) return BadRequest("Cannot delete this tag (maybe it's linked to articles).");
            return Ok("Tag deleted.");
        }
    }
}

