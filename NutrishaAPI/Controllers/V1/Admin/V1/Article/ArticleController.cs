﻿using System;
using System.Threading.Tasks;
using DL.DtosV1.Articles;
using DL.ResultModels;
using DL.Services.Blogs.Articles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Attributes;
using NutrishaAPI.Controllers.V1.Admin.V1;
using NutrishaAPI.Validations.Articles;

namespace NutrishaAPI.Controllers.V1.Admin.V1.Article
{
// [OnlyAdmins]
    [Authorize]
    public class ArticleController : BaseAdminV1Controller
    {
        private readonly ArticleService _articleService;

        public ArticleController(ArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpPost("Post")]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue,
            MultipartHeadersLengthLimit = int.MaxValue)]
        public async Task<IActionResult> PostAsync([FromForm] PostArticleDto postArticleDto)
        {
            var validationResult = postArticleDto.IsValid();
            if (!validationResult.Success)
            {
                return InvalidResult(validationResult.Errors);
            }

            var result = await _articleService.PostAsync(postArticleDto);
            return ItemResult(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> PostAsync([FromQuery] long id)
        {
            if (id < 1)
            {
                return InvalidResult(NonLocalizedErrorMessages.InvalidId);
            }

            var result = await _articleService.GetByIdForAdmin(id);
            return ItemResult(result);
        }


        [HttpPut("Edit")]
        public async Task<IActionResult> EditAsync(EditArticleDto editArticleDto)
        {
            var validationResult = editArticleDto.IsValid();
            if (!validationResult.Success)
            {
                return InvalidResult(validationResult.Errors);
            }

            // await _articleService.PutAsync();

            return Ok();
        }
    }
}