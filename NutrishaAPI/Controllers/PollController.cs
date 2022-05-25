using AutoMapper;
using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using DL.DTOs.PollDTO;
using DL.DTOs.PollDTO;
using DL.DTOs.PollDTO;
using DL.DTOs.UserDTOs;
using DL.DTOs.UserPollAnswerDTO;
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
    [Authorize]
    //[ClaimRequirement(ClaimTypes.Role, RoleConstant.Admin)]
    public class PollController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IUserManagementService _UserManagementService;//**
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;

        public PollController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger, IUserManagementService UserManagementService)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _UserManagementService = UserManagementService;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }


        [HttpPost, Route("AddUserPollAnswer")]
        [ProducesResponseType(typeof(MUserPollAnswer), StatusCodes.Status200OK)]
        public IActionResult AddUserPollAnswer(UserPollAnswerCreatDto articleLikeCreatDto)
        {
            var userId = Convert.ToInt32(_UserManagementService.getUserId(this.User.Identity.Name));
            var articleLike = _uow.UserPollAnswerRepository.GetMany(c => c.UserId == userId && c.PollAnswerId == articleLikeCreatDto.PollAnswerId).FirstOrDefault();
            if (articleLike != null)
            {
                baseResponse.data = "You already Select Answer ";
                baseResponse.statusCode = (int)HttpStatusCode.NotFound;  // Errors.Success;
                baseResponse.done = false;
                return Ok(baseResponse);
            }
            else
            {

                var newUserPollAnswer = new MUserPollAnswer();
                //  availability.Mobile = user.Mobile;
                newUserPollAnswer.UserId = userId;
                newUserPollAnswer.PollAnswerId = articleLikeCreatDto.PollAnswerId;
             //   newUserPollAnswer.Answer = articleLikeCreatDto.Answer;
                try
                {
                    _uow.UserPollAnswerRepository.Add(newUserPollAnswer);
                    _uow.Save();
                    baseResponse.data = newUserPollAnswer;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;  // Errors.Success;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
               catch (Exception ex)
                {
                    baseResponse.data = "User Or Poll Not Found";
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;  // Errors;
                    baseResponse.done = false;
                    return Ok(baseResponse);
                }
           
            }



        }

  


    }
}
