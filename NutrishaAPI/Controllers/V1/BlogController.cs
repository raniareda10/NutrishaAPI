using System;
using System.Threading.Tasks;
using DL.EntitiesV1.Blogs;
using DL.Enums;
using DL.ResultModels;
using DL.Services.Blogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Bases;
using NutrishaAPI.Validations.Shared;

namespace NutrishaAPI.Controllers.V1
{
    [Authorize]
    public class BlogController : BaseMobileController
    {
        private readonly BlogService _blogService;

        public BlogController(BlogService BlogService)
        {
            _blogService = BlogService;
        }

        [HttpGet("GetPagedList")]
        public async Task<IActionResult> GetPagedListAsync([FromQuery] BlogTimelinePagedModel model)
        {
            if (!model.IsValidPagedModel())
                return InvalidResult(ErrorMessages.InvalidParameters);

            return PagedResult(await _blogService.GetTimelineAsync(model));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] long id, EntityType entityType)
        {
            if (id == 0) return InvalidResult(ErrorMessages.InvalidId);
            
            return ObjectResult( await _blogService.GetBlogByIdAsync(id, entityType));
        }
    }
}