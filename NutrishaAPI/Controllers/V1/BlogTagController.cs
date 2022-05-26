using System.Threading.Tasks;
using DL.Services.Blogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Bases;

namespace NutrishaAPI.Controllers.V1
{
    [Authorize]
    public class BlogTagController : BaseMobileController
    {
        private readonly BlogTagService _blogTagService;

        public BlogTagController(BlogTagService blogTagService)
        {
            _blogTagService = blogTagService;
        }

        [Route("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return ListResult(await _blogTagService.GetAllTags());
        }
    }
}