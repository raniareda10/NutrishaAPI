using System.Threading.Tasks;
using DL.EntitiesV1.Blogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Bases;

namespace NutrishaAPI.Controllers.V1
{
    public class BlogController : BaseMobileController
    {
        private readonly BlogTimelineService _blogTimelineService;

        public BlogController(BlogTimelineService blogTimelineService)
        {
            _blogTimelineService = blogTimelineService;
        }
        
        [HttpGet("GetPagedList")]
        [Authorize]
        public async Task<IActionResult> GetPagedListAsync([FromQuery] BlogTimelinePagedModel model)
        {
            return PagedResult(await _blogTimelineService.GetTimelineAsync(model));
        }
    }
}