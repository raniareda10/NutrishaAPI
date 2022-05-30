using System.Threading.Tasks;
using DL.DtosV1.Articles;
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
        public async Task<IActionResult> PostAsync([FromForm()] PostArticleDto postArticleDto)
        {
            var q = HttpContext;
            var validationResult = postArticleDto.IsValid();
            if (!validationResult.Success)
            {
                return InvalidResult(validationResult.Errors);
            }
            
            var result = await _articleService.PostAsync(postArticleDto);
            return ItemResult(result);
        }
    }
}