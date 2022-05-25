using AutoMapper;
using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using DL.DTOs.VideoDTO;
using DL.DTOs.VideoDTO;
using DL.DTOs.PollDTO;
using DL.DTOs.UserDTOs;
using DL.Entities;
using DL.MailModels;
using HELPER;
using Helpers;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using Model.ApiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using NutrishaAPI.Extensions;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    //[ClaimRequirement(ClaimTypes.Role, RoleConstant.Admin)]
    public class VideoController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;

        public VideoController(IUnitOfWork uow, IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger)
        {
            _uow = uow;
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }




        [HttpGet]
        [ProducesResponseType(typeof(AllVideoDto), StatusCodes.Status200OK)]
        public IActionResult GetAllVideo([FromQuery] VideoQueryPramter VideoQueryPramter)
        {
            try
            {
                var lstVideo = _uow.VideoRepository.GetAllVideo(VideoQueryPramter);
                List<AllVideoDto> AllVideo = new List<AllVideoDto>();
                foreach (var item in lstVideo)
                {
                    var dateNow = DateTime.Now;
                    var user = _uow.SecUserRepository.GetAll().Where(c => c.Id == item.SecUserId).FirstOrDefault();
                  //  var lstVideoLike = _uow.VideoLikeRepository.GetAll().Where(c => c.VideoId == item.Id).ToList();
                    AllVideoDto videoDTO = new AllVideoDto()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        //  VideoContent = item.VideoContent,
                        CreatedTime = item.CreatedTime,
                        Notes = item.Notes,

                    };
                    if (user != null)
                    {
                        videoDTO.AdminName = user.Name;
                    }
              
                    if (item.Video != null)
                    {
                        videoDTO.Video = $"https://nutrisha.app/Video/{item.Video}";
                    }
                    AllVideo.Add(videoDTO);
                }
                baseResponse.data = AllVideo;
                baseResponse.total_rows = AllVideo.Count();
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;
                return Ok(baseResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllVideos action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpGet("{id}", Name = "VideoById")]
        [ProducesResponseType(typeof(MVideo), StatusCodes.Status200OK)]
        public IActionResult GetAllVideoId(int id)
        {
            try
            {
                var Video = _uow.VideoRepository.GetById(id);
                if (Video == null)
                {
                    _logger.LogError($"Video with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Not Found";
                    return NotFound(baseResponse);
                }
                else
                {

                    var dateNow = DateTime.Now;
                    var user = _uow.SecUserRepository.GetAll().Where(c => c.Id == Video.SecUserId).FirstOrDefault();
                    //  var lstVideoLike = _uow.VideoLikeRepository.GetAll().Where(c => c.VideoId == item.Id).ToList();
                    AllVideoDto videoDTO = new AllVideoDto()
                    {
                        Id = Video.Id,
                        Title = Video.Title,
                        Link = Video.Link,
                        CreatedTime = Video.CreatedTime,
                        Notes = Video.Notes,

                    };
                    if (user != null)
                    {
                        videoDTO.AdminName = user.Name;
                    }

                    if (Video.Video != null)
                    {
                        videoDTO.Video = $"https://nutrisha.app/Video/{Video.Video}";
                    }
       
                baseResponse.data = videoDTO;
                    baseResponse.total_rows = 1;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetVideoById action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



    }
}
