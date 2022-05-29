using System.Threading.Tasks;
using DL.Services.Blogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    [Authorize]
    public class BlogTagController : BaseMobileController
    {
        private readonly BlogTagService _blogTagService;

        public BlogTagController(BlogTagService blogTagService)
        {
            _blogTagService = blogTagService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return ListResult(await _blogTagService.GetAllTags());
        }
    }
}