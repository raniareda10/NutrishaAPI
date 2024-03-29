﻿using System.Threading.Tasks;
using DL.EntitiesV1.Blogs;
using DL.Enums;
using DL.Repositories.Blogs;
using DL.ResultModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Mobile.Bases;
using NutrishaAPI.Validations.Shared;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    [Authorize]
    public class BlogController : BaseMobileController
    {
        private readonly BlogService _blogService;

        public BlogController(BlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet("GetPagedList")]
        public async Task<IActionResult> GetPagedListAsync([FromQuery] BlogTimelinePagedModel model)
        {
            if (!model.IsValidPagedModel())
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);

            return PagedResult(await _blogService.GetTimelineAsync(model));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] long id, EntityType entityType)
        {
            if (id == 0) return InvalidResult(NonLocalizedErrorMessages.InvalidId);
            
            return ItemResult( await _blogService.GetBlogByIdAsync(id, entityType));
        }
    }
}