using System.Threading.Tasks;
using DL.DtosV1.BlogTags;
using DL.Repositories.Blogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Validations.Blogs;

namespace NutrishaAPI.Controllers.V1.Admin.V1
{
    [Authorize]
    public class BlogTagController : BaseAdminV1Controller
    {
        private readonly BlogTagService _blogTagService;

        public BlogTagController(BlogTagService blogTagService)
        {
            _blogTagService = blogTagService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync([FromQuery] string keyword)
        {
            return ListResult(await _blogTagService.GetAllTags(keyword));
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(PostBlogTagDto postBlogTagDto)
        {
            var validationResult = postBlogTagDto.IsValid();

            if (!validationResult.Success)
                return InvalidResult(validationResult.Errors);

            return ItemResult(await _blogTagService.Post(postBlogTagDto));
        }
    }
}