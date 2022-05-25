using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using BL.Infrastructure;
using BL.Security;
using DL.DTOs.ArticleAttachmentDTO;
using DL.DTOs.ArticleCommentDTO;
using DL.DTOs.ArticleDTO;
using DL.DTOs.ArticleLikeDTO;
using DL.Entities;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Extensions;

namespace NutrishaAPI.Controllers.LegacyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    [Authorize]
    //[ClaimRequirement(ClaimTypes.Role, RoleConstant.Admin)]
    public class ArticleController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;
        private readonly IUserManagementService _UserManagementService;//**

        public ArticleController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger, IUserManagementService UserManagementService)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
            _UserManagementService = UserManagementService;
        }


        [HttpGet("{id}", Name = "ArticleById")]
        [ProducesResponseType(typeof(MArticle), StatusCodes.Status200OK)]
        public IActionResult GetAllArticleId(int id)
        {
            try
            {
                var userId = Convert.ToInt32(_UserManagementService.getUserId(this.User.Identity.Name));
                var Article = _uow.ArticleRepository.GetById(id);
                if (Article == null)
                {
                    _logger.LogError($"Article with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Not Found";
                    return NotFound(baseResponse);
                }
                else
                {

                    var dateNow = DateTime.Now;
                    var lstArticleComment = _uow.ArticleCommentRepository.GetAll().Where(c => c.ArticleId == Article.Id).ToList();
                    var user = _uow.SecUserRepository.GetAll().Where(c => c.Id == Article.SecUserId).FirstOrDefault();
                    var lstArticleLike = _uow.ArticleLikeRepository.GetAll().Where(c => c.ArticleId == Article.Id).ToList();
                    var lstArticleAttachment = _uow.ArticleAttachmentRepository.GetAll().Where(c => c.ArticleId == Article.Id).ToList();
                    var lstArticleCommentId = lstArticleComment.Select(c=>c.Id);
                    var lstArticleCommentLike = _uow.ArticleCommentLikeRepository.GetAll().Where(c => lstArticleCommentId.Contains(c.ArticleCommentId)).ToList();
                    var lstArticleCommentLikeUserId = lstArticleCommentLike.Select(c=>c.UserId).ToList();

                    AllArticleDto articleDTO = new AllArticleDto()
                    {
                        Id = Article.Id,
                        Subject = Article.Subject,
                        ArticleContent = Article.ArticleContent,
                        CreatedTime = Article.CreatedTime,
                        Notes = Article.Notes,

                    };
                    articleDTO.MediaTypeId = 1;
                    if (user != null)
                    {
                        var owner = new OwnerDto();
                        owner.Id = user.Id;
                        owner.Name = user.Name;
                        owner.ImageUrl = user.Image;
                        articleDTO.owner = owner;
                    }
                    if (lstArticleComment.Any())
                    {
                        articleDTO.CommentsCount = lstArticleComment.Count();
                    }
                    if (lstArticleLike.Any())
                    {
                        articleDTO.LikesCount = lstArticleLike.Count();
                    }
                    if (lstArticleComment.Any())
                    {
                        var lstArticleCommentDto = new List<IncludeArticleCommentDto>();
                        foreach (var itemComment in lstArticleComment)
                        {
                            var includeArticleCommentDto = new IncludeArticleCommentDto();
                            includeArticleCommentDto.Comment = itemComment.Comment;
                            includeArticleCommentDto.Date = itemComment.Date;
                            if (lstArticleCommentLike.Any())
                            {
                                includeArticleCommentDto.LikesCount = lstArticleCommentLike.Count();
                            }
                            if (lstArticleCommentLikeUserId.Contains(userId))
                            {
                                includeArticleCommentDto.HasLike = true;
                            }
                            lstArticleCommentDto.Add(includeArticleCommentDto);
                        }
                        articleDTO.LstArticleComment = lstArticleCommentDto;
                    }
                    if (Article.CoverImage != null)
                    {
                        articleDTO.CoverImage = $"https://nutrisha.app/Picture/{Article.CoverImage}";
                    }
                    if (lstArticleAttachment.Any())
                    {
                        var lstArticleAttachmentDto = new List<IncludeArticleAttachmentDto>();
                        foreach (var itemAttachment in lstArticleAttachment)
                        {
                            var includeArticleAttachmentDto = new IncludeArticleAttachmentDto();
                            includeArticleAttachmentDto.AttachmentTypeId = itemAttachment.AttachmentTypeId;
                            includeArticleAttachmentDto.Url = itemAttachment.Url;                      
                            lstArticleAttachmentDto.Add(includeArticleAttachmentDto);
                        }
                        articleDTO.attachment = lstArticleAttachmentDto;
                    }
                    baseResponse.data = articleDTO;
                    baseResponse.total_rows = 1;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetArticleById action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }






       
        [HttpPost, Route("AddArticleLike")]
        [ProducesResponseType(typeof(MArticleLike), StatusCodes.Status200OK)]
        public IActionResult AddArticleLike(ArticleLikeCreatDto articleLikeCreatDto)
        {
            var userId = Convert.ToInt32(_UserManagementService.getUserId(this.User.Identity.Name));
            var articleLike = _uow.ArticleLikeRepository.GetMany(c => c.UserId == userId && c.ArticleId == articleLikeCreatDto.ArticleId).FirstOrDefault();
            if (articleLike != null)
            {
                baseResponse.data = "You already Pressed  Like ";
                baseResponse.statusCode = (int)HttpStatusCode.NotFound;  // Errors.Success;
                baseResponse.done = false;
                return Ok(baseResponse);
            }
            else
            {

                var newArticleLike = new MArticleLike();
                //  availability.Mobile = user.Mobile;
                newArticleLike.UserId = userId;
                newArticleLike.ArticleId = articleLikeCreatDto.ArticleId;
                try
                {
                    _uow.ArticleLikeRepository.Add(newArticleLike);
                    _uow.Save();
                    baseResponse.data = newArticleLike;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;  // Errors.Success;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
               catch (Exception ex)
                {
                    baseResponse.data = "User Or Article Not Found";
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;  // Errors;
                    baseResponse.done = false;
                    return Ok(baseResponse);
                }
           
            }



        }
        [HttpPost, Route("AddArticleCommentLike")]
        [ProducesResponseType(typeof(MArticleCommentLike), StatusCodes.Status200OK)]
        public IActionResult AddArticleCommentLike(ArticleCommentLikeCreatDto articleCommentLikeCreatDto)
        {
            var userId = Convert.ToInt32(_UserManagementService.getUserId(this.User.Identity.Name));

            var articleCommentLike = _uow.ArticleCommentLikeRepository.GetMany(c => c.UserId == userId && c.ArticleCommentId == articleCommentLikeCreatDto.ArticleCommentId).FirstOrDefault();
            if (articleCommentLike != null)
            {
                baseResponse.data = "You already Pressed  Like ";
                baseResponse.statusCode = (int)HttpStatusCode.NotFound;  // Errors.Success;
                baseResponse.done = false;
                return Ok(baseResponse);
            }
            else
            {

                var newArticleCommentLike = new MArticleCommentLike();
                //  availability.Mobile = user.Mobile;
                newArticleCommentLike.UserId = userId;
                newArticleCommentLike.ArticleCommentId = articleCommentLikeCreatDto.ArticleCommentId;
                try
                {
                    _uow.ArticleCommentLikeRepository.Add(newArticleCommentLike);
                    _uow.Save();
                    baseResponse.data = newArticleCommentLike;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;  // Errors.Success;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
                catch (Exception ex)
                {
                    baseResponse.data = "User Or ArticleComment Not Found";
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;  // Errors;
                    baseResponse.done = false;
                    return Ok(baseResponse);
                }

            }



        }



        [HttpGet, Route("AllArticleComment")]
        [ProducesResponseType(typeof(AllArticleCommentDto), StatusCodes.Status200OK)]
        public IActionResult GetAllArticleComment([FromQuery] ArticleCommentQueryPramter ArticleCommentQueryPramter)
        {
            try
            {
                var userId = Convert.ToInt32(_UserManagementService.getUserId(this.User.Identity.Name));
                var lstArticleComment = _uow.ArticleCommentRepository.GetAllArticleComment(ArticleCommentQueryPramter);
                var lstArticleCommentId = lstArticleComment.Select(c => c.Id);
                var lstArticleCommentLike = _uow.ArticleCommentLikeRepository.GetAll().Where(c => lstArticleCommentId.Contains(c.ArticleCommentId)).ToList();
                var lstArticleCommentLikeUserId = lstArticleCommentLike.Select(c => c.UserId).ToList();
                var lstArticleCommentDto = new List<IncludeArticleCommentDto>();
                if (lstArticleComment.Any())
                    {
                     
                        foreach (var itemComment in lstArticleComment)
                        {
                            var includeArticleCommentDto = new IncludeArticleCommentDto();
                            includeArticleCommentDto.Comment = itemComment.Comment;
                            includeArticleCommentDto.Date = itemComment.Date;
                            if (lstArticleCommentLike.Any())
                            {
                                includeArticleCommentDto.LikesCount = lstArticleCommentLike.Count();
                            }
                            if (lstArticleCommentLikeUserId.Contains(userId))
                            {
                                includeArticleCommentDto.HasLike = true;
                            }
                            lstArticleCommentDto.Add(includeArticleCommentDto);
                        }

                }
                baseResponse.data = lstArticleCommentDto;
                baseResponse.total_rows = lstArticleCommentDto.Count();
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;
                return Ok(baseResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllArticleComments action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpGet, Route("ArticleCommentById")]
        [ProducesResponseType(typeof(MArticleComment), StatusCodes.Status200OK)]
        public IActionResult GetAllArticleCommentId(int id)
        {
            try
            {
                var ArticleComment = _uow.ArticleCommentRepository.GetById(id);
                if (ArticleComment == null)
                {
                    _logger.LogError($"ArticleComment with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Not Found";
                    return NotFound(baseResponse);
                }
                else
                {

                    var lstArticleCommentComment = _uow.ArticleCommentRepository.GetAll().Where(c => c.ParentId == ArticleComment.Id).ToList();
                    var lstArticleCommentLike = _uow.ArticleCommentLikeRepository.GetAll().Where(c => c.ArticleCommentId == ArticleComment.Id).ToList();
                    AllArticleCommentDto articleCommentDTO = new AllArticleCommentDto()
                    {
                        Id = ArticleComment.Id,
                        ArticleId = ArticleComment.ArticleId,
                        Comment = ArticleComment.Comment,
                        Date = ArticleComment.Date,
                        Notes = ArticleComment.Notes,

                    };

                    if (lstArticleCommentComment.Any())
                    {
                        articleCommentDTO.CommentCount = lstArticleCommentComment.Count();
                    }
                    if (lstArticleCommentLike.Any())
                    {
                        articleCommentDTO.LikeCount = lstArticleCommentLike.Count();
                    }
                    if (lstArticleCommentComment.Any())
                    {
                        articleCommentDTO.LstArticleCommentReply = lstArticleCommentComment;
                    }

                    baseResponse.data = articleCommentDTO;
                    baseResponse.total_rows = 1;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetArticleCommentById action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }
        [HttpPost, Route("ArticleComment")]
        [ProducesResponseType(typeof(ArticleCommentCreatDto), StatusCodes.Status201Created)]
        public IActionResult CreateArticleComment(ArticleCommentCreatDto ArticleComment)
        {
            try
            {
                var userId = Convert.ToInt32(_UserManagementService.getUserId(this.User.Identity.Name));
                if (ArticleComment == null)
                {
                    _logger.LogError("ArticleComment object sent from articleComment is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid ArticleComment object sent from articleComment.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid model object";
                    return NotFound(baseResponse);
                }
                var ArticleCommentEntity = _mapper.Map<MArticleComment>(ArticleComment);

                _uow.ArticleCommentRepository.Add(ArticleCommentEntity);
                _uow.Save();
                var createdArticleComment = _mapper.Map<ArticleCommentCreatDto>(ArticleCommentEntity);
                baseResponse.data = createdArticleComment;
                baseResponse.statusCode = StatusCodes.Status201Created;
                baseResponse.done = true;
                return CreatedAtRoute("ArticleCommentById", new { id = ArticleCommentEntity.Id }, baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateArticleComment action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }

        [HttpPut, Route("ArticleComment")]
        [ProducesResponseType(typeof(MArticleComment), StatusCodes.Status200OK)]
        public IActionResult UpdateArticleComment(int id, [FromBody] ArticleCommentCreatDto ArticleComment)
        {
            try
            {
                if (ArticleComment == null)
                {
                    _logger.LogError("ArticleComment object sent from articleComment is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid ArticleComment object sent from articleComment.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid";
                    return NotFound(baseResponse);
                }
                var ArticleCommentEntity = _uow.ArticleCommentRepository.GetById(id);
                if (ArticleCommentEntity == null)
                {
                    _logger.LogError($"ArticleComment with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                _mapper.Map(ArticleComment, ArticleCommentEntity);
                _uow.ArticleCommentRepository.Update(ArticleCommentEntity);
                _uow.Save();
                baseResponse.data = ArticleCommentEntity;
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateArticleComment action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpDelete, Route("ArticleComment")]
        [ProducesResponseType(typeof(MArticleComment), StatusCodes.Status200OK)]
        public IActionResult DeleteArticleComment(int id)
        {
            try
            {
                var ArticleComment = _uow.ArticleCommentRepository.GetById(id);
                if (ArticleComment == null)
                {
                    _logger.LogError($"ArticleComment with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "hasn't been found in db";
                    return NotFound(baseResponse);
                }
                else
                {
                    var lstArticleCommentComment = _uow.ArticleCommentRepository.GetAll().Where(c => c.ParentId == ArticleComment.Id).ToList();
                    foreach (var item in lstArticleCommentComment)
                    {
                        _uow.ArticleCommentRepository.Delete(item.Id);
                        _uow.Save();
                    }
                        _uow.ArticleCommentRepository.Delete(id);
                    _uow.Save();
                    baseResponse.statusCode = StatusCodes.Status200OK;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteArticleComment action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }
    }
}
