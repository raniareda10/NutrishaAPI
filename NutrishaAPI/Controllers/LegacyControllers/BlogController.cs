using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using BL.Infrastructure;
using BL.Security;
using DL.DTOs.ArticleAttachmentDTO;
using DL.DTOs.ArticleDTO;
using DL.DTOs.PollAnswerDTO;
using DL.DTOs.PollDTO;
using DL.DTOs.VideoDTO;
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
    public class BlogController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;
        private readonly IUserManagementService _UserManagementService;//**
   
        public BlogController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger, IUserManagementService UserManagementService)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
            _UserManagementService = UserManagementService;
        }



        
        [HttpGet]
        [ProducesResponseType(typeof(AllBlogDto), StatusCodes.Status200OK)]
        public IActionResult GetAllBlog([FromQuery] BlogQueryPramter BlogQueryPramter)
        {
            try
            {
                var userId = Convert.ToInt32(_UserManagementService.getUserId(this.User.Identity.Name));
                var lstArticle = _uow.ArticleRepository.GetAll().Where(c=>c.BlogTypeId == BlogQueryPramter.BlogTypeId).OrderByDescending(c => c.CreatedTime).Take(5).ToList();
                var lstPoll = _uow.PollRepository.GetAll().Where(c => c.BlogTypeId == BlogQueryPramter.BlogTypeId).OrderByDescending(c => c.CreatedTime).Take(5).ToList();
                var lstVideo = _uow.VideoRepository.GetAll().Where(c => c.BlogTypeId == BlogQueryPramter.BlogTypeId).OrderByDescending(c => c.CreatedTime).Take(5).ToList();
                List<AllBlogDto> AllBlog = new List<AllBlogDto>();
                foreach (var item in lstArticle)
                {
                    var dateNow = DateTime.Now;
                    var lstArticleComment = _uow.ArticleCommentRepository.GetAll().Where(c=>c.ArticleId == item.Id).Select(c=>c.UserId).ToList();
                    var lstArticleLike = _uow.ArticleLikeRepository.GetAll().Where(c => c.ArticleId == item.Id).Select(c => c.UserId).ToList();
                    var lstArticleAttachment = _uow.ArticleAttachmentRepository.GetAll().Where(c => c.ArticleId == item.Id).ToList();
                    var user = _uow.SecUserRepository.GetAll().Where(c => c.Id == item.SecUserId).FirstOrDefault();
            
                    AllBlogDto blogDTO = new AllBlogDto()
                    {
                        Id = item.Id,
                        Subject = item.Subject,                   
                        BlogEntityTypeId = 1,
                        CreatedTime = item.CreatedTime,
                    };
                    if (item.CoverImage != null)
                    {
                        blogDTO.CoverImage = $"https://nutrisha.app/Picture/{item.CoverImage}";
                    }
                    if (user != null)
                    {
                        var owner = new OwnerDto();
                        owner.Id = user.Id;
                        owner.Name = user.Name;
                        owner.ImageUrl = user.Image;
                        blogDTO.owner = owner;
                    }
                    var article = new ArticleDto();
                    article.ArticleContent = item.ArticleContent;

                    if (lstArticleComment.Any())
                    {
                        article.CommentsCount = lstArticleComment.Count();
                    }
                    if (lstArticleLike.Any())
                    {
                        article.LikesCount = lstArticleLike.Count();
                    }
                    if (lstArticleComment.Contains(userId))
                    {
                        article.HasComment = true;
                    }
                    if (lstArticleLike.Contains(userId))
                    {
                        article.HasLike = true;
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
                            article.attachment = lstArticleAttachmentDto;
                    }
                    blogDTO.article = article;
                    AllBlog.Add(blogDTO);
                }
                foreach (var item in lstPoll)
                {
                    var dateNow = DateTime.Now;
                    var lstPollAnswer = _uow.PollAnswerRepository.GetAll().Where(c => c.PollId == item.Id).ToList();
                    var lstPollAnswerId = lstPollAnswer.Select(c => c.Id).ToList();
                    var lstUserPollAnswer = _uow.UserPollAnswerRepository.GetAll().Where(c => lstPollAnswerId.Contains(c.PollAnswerId)).ToList();

                    var user = _uow.SecUserRepository.GetAll().Where(c => c.Id == item.SecUserId).FirstOrDefault();

                    AllBlogDto blogDTO = new AllBlogDto()
                    {
                        Id = item.Id,
                        Subject = item.Question,
                        BlogEntityTypeId = 2,
                        CreatedTime = item.CreatedTime,
                    };
                
                    if (user != null)
                    {
                        var owner = new OwnerDto();
                        owner.Id = user.Id;
                        owner.Name = user.Name;
                        owner.ImageUrl = user.Image;
                        blogDTO.owner = owner;
                    }
                    var poll = new PollDto();
                    if (lstUserPollAnswer.Where(c => c.UserId == userId).FirstOrDefault() != null)
                    {
                        poll.selectedAnswerId = lstUserPollAnswer.Where(c => c.UserId == userId).FirstOrDefault().PollAnswerId;
                    }
                    if (lstPollAnswer.Any())
                    {
                        var lstPollAnswerDto = new List<IncludePollAnswerDto>();
                        foreach (var itemAnswer in lstPollAnswer)
                        {
                            var includePollAnswerDto = new IncludePollAnswerDto();
                            includePollAnswerDto.Id = itemAnswer.Id;
                            includePollAnswerDto.Name = itemAnswer.Name;
                            includePollAnswerDto.Color = itemAnswer.Color;
                            if (lstUserPollAnswer.Count > 0)
                            {
                                includePollAnswerDto.SelectionRate = (decimal)(lstUserPollAnswer.Where(c => c.PollAnswerId == itemAnswer.Id).Count() * 100 / lstUserPollAnswer.Count) ;
                            }
                                lstPollAnswerDto.Add(includePollAnswerDto);
                        }
                        poll.options = lstPollAnswerDto;
                    }
                    blogDTO.poll = poll;
                    AllBlog.Add(blogDTO);
                }
                foreach (var item in lstVideo)
                {
                    var dateNow = DateTime.Now;
                    var user = _uow.SecUserRepository.GetAll().Where(c => c.Id == item.SecUserId).FirstOrDefault();

                    AllBlogDto blogDTO = new AllBlogDto()
                    {
                        Id = item.Id,
                        Subject = item.Title,
                        BlogEntityTypeId = 3,
                        CreatedTime = item.CreatedTime,
                    };
                    if (item.CoverImage != null)
                    {
                        blogDTO.CoverImage = $"https://nutrisha.app/Picture/{item.CoverImage}";
                    }
                    if (user != null)
                    {
                        var owner = new OwnerDto();
                        owner.Id = user.Id;
                        owner.Name = user.Name;
                        owner.ImageUrl = user.Image;
                        blogDTO.owner = owner;
                    }

                    var video = new VideoDto();
                        var includeVideoAttachmentDto = new IncludeArticleAttachmentDto();
                            includeVideoAttachmentDto.AttachmentTypeId = item.AttachmentTypeId;
                            includeVideoAttachmentDto.Url = item.Video;
                        
                        video.attachment = includeVideoAttachmentDto;
                    blogDTO.video = video;
                    AllBlog.Add(blogDTO);
                }
                baseResponse.data = AllBlog;
                baseResponse.total_rows = AllBlog.Count();
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;
                return Ok(baseResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllBlogs action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



    }
}
