using AutoMapper;
using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using DL.DTOs.NotificationDTO;
using DL.DTOs.NotificationUserDTO;
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


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    //[ClaimRequirement(ClaimTypes.Role, RoleConstant.Admin)]
    public class NotificationController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;

        public NotificationController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }



        
        [HttpGet]
        [ProducesResponseType(typeof(IQueryable<AllNotificationDto>), StatusCodes.Status200OK)]
        public IActionResult GetAllNotificationUser([FromQuery] NotificationUserQueryPramter NotificationQueryPramter)
        {
            try
            {
                var lstNotification = _uow.NotificationUserRepository.GetAllNotificationUser(NotificationQueryPramter);
                List<AllNotificationDto> AllNotification = new List<AllNotificationDto>();
                foreach (var item in lstNotification)
                {
      
                    var notification = _mapper.Map<DL.DTOs.NotificationDTO.IncludeNotificationDto>(item.Notification);
                    //var user = _mapper.Map<DL.DTOs.UserDTO.IncludeUserDto>(item.User);
                    AllNotificationDto orderDTO = new AllNotificationDto()
                    {
                        NotificationId = item.NotificationId,
                        UserId = item.UserId,
                        Message = notification.Message,
                        Description = notification.Description,
                        Date = notification.Date.ToString("yyyy-MM-ddTHH:mm"),     
                        IsSeen = notification.IsSeen,
                    
                    };

                    AllNotification.Add(orderDTO);
                }
                baseResponse.data = AllNotification;
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;
                return Ok(baseResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllNotifications action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.errorMessage = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpGet("{id}", Name = "NotificationById")]
        [ProducesResponseType(typeof(IQueryable<MNotification>), StatusCodes.Status200OK)]
        public IActionResult GetAllNotificationId(int id)
        {
            try
            {
                var Notification = _uow.NotificationRepository.GetById(id);
                if (Notification == null)
                {
                    _logger.LogError($"Notification with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.errorMessage = "Not Found";
                    return NotFound(baseResponse);
                }
                else
                {
                    _logger.LogInfo($"Returned Notification with id: {id}");
                    //var NotificationResult = _mapper.Map<NotificationDTO>(Notification);
                    baseResponse.data = Notification;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;
                    baseResponse.done = true;
                    return Ok(Notification);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetNotificationById action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.errorMessage = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



 

  
        //[HttpGet, Route("ActivateNotification")]
        //public IActionResult ActivateNotification(int Id)
        //{
        //    try
        //    {
        //        var Notification = _uow.NotificationRepository.GetById(Id);
        //        Notification.IsActive = true;
        //        _uow.NotificationRepository.Update(Notification);
        //        _uow.Save();
        //        return Content("Notification Activated");
        //    }
        //    catch (Exception ex)
        //    {

        //        return Content(ex.ToString());
        //    }


        //}




        //[HttpPost]
        //[ProducesResponseType(typeof(NotificationCreatDto), StatusCodes.Status201Created)]
        //public IActionResult CreateNotification([FromForm] NotificationCreatDto Notification)
        //{
        //    try
        //    {
        //        if (Notification == null)
        //        {
        //            _logger.LogError("Notification object sent from notification is null.");
        //            baseResponse.done = false;
        //            baseResponse.statusCode = (int)HttpStatusCode.NotFound;
        //            baseResponse.errorMessage = "object is Null";
        //            return NotFound(baseResponse);
        //        }
        //        if (!ModelState.IsValid)
        //        {
        //            _logger.LogError("Invalid Notification object sent from notification.");
        //            baseResponse.done = false;
        //            baseResponse.statusCode = (int)HttpStatusCode.NotFound;
        //            baseResponse.errorMessage = "Invalid model object";
        //            return NotFound(baseResponse);
        //        }
        //        var NotificationEntity = _mapper.Map<MNotification>(Notification);
               
        //        _uow.NotificationRepository.Add(NotificationEntity);
        //        _uow.Save();
        //        var createdNotification = _mapper.Map<NotificationCreatDto>(NotificationEntity);
        //        baseResponse.data = createdNotification;
        //        baseResponse.statusCode = StatusCodes.Status201Created;
        //        baseResponse.done = true;
        //        return CreatedAtRoute("NotificationById", new { id = NotificationEntity.Id }, baseResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Something went wrong inside CreateNotification action: {ex.Message}");
        //        baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
        //        baseResponse.done = false;
        //        baseResponse.errorMessage = $"Exception :{ex.Message}";
        //        return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
        //    }
        //}


        //[HttpPut("{id}")]
        //[ProducesResponseType(typeof(IQueryable<MNotification>), StatusCodes.Status200OK)]
        //public IActionResult UpdateNotification(int id, [FromBody] NotificationCreatDto Notification)
        //{
        //    try
        //    {
        //        if (Notification == null)
        //        {
        //            _logger.LogError("Notification object sent from notification is null.");
        //            baseResponse.done = false;
        //            baseResponse.statusCode = (int)HttpStatusCode.NotFound;
        //            baseResponse.errorMessage = "object is Null";
        //            return NotFound(baseResponse);
        //        }
        //        if (!ModelState.IsValid)
        //        {
        //            _logger.LogError("Invalid Notification object sent from notification.");
        //            baseResponse.done = false;
        //            baseResponse.statusCode = (int)HttpStatusCode.NotFound;
        //            baseResponse.errorMessage = "Invalid";
        //            return NotFound(baseResponse);
        //        }
        //        var NotificationEntity = _uow.NotificationRepository.GetById(id);
        //        if (NotificationEntity == null)
        //        {
        //            _logger.LogError($"Notification with id: {id}, hasn't been found in db.");
        //            baseResponse.done = false;
        //            baseResponse.statusCode = (int)HttpStatusCode.NotFound;
        //            baseResponse.errorMessage = "object is Null";
        //            return NotFound(baseResponse);
        //        }
        //        _mapper.Map(Notification, NotificationEntity);
        //        _uow.NotificationRepository.Update(NotificationEntity);
        //        _uow.Save();
        //        baseResponse.data = NotificationEntity;
        //        baseResponse.statusCode = StatusCodes.Status200OK;
        //        baseResponse.done = true;
        //        return Ok(baseResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Something went wrong inside UpdateNotification action: {ex.Message}");
        //        baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
        //        baseResponse.done = false;
        //        baseResponse.errorMessage = $"Exception :{ex.Message}";
        //        return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
        //    }
        //}



        //[HttpDelete("{id}")]
        //[ProducesResponseType(typeof(IQueryable<MNotification>), StatusCodes.Status200OK)]
        //public IActionResult DeleteNotification(int id)
        //{
        //    try
        //    {
        //        var Notification = _uow.NotificationRepository.GetById(id);
        //        if (Notification == null)
        //        {
        //            _logger.LogError($"Notification with id: {id}, hasn't been found in db.");
        //            baseResponse.done = false;
        //            baseResponse.statusCode = (int)HttpStatusCode.NotFound;
        //            baseResponse.errorMessage = "hasn't been found in db";
        //            return NotFound(baseResponse);
        //        }

        //        //if (_repository.NotificationClaim.NotificationClaimByNotification(id).Any())
        //        //{
        //        //    _logger.LogError($"Cannot delete Notification with id: {id}. It has related accounts. Delete those accounts first");
        //        //    return BadRequest("Cannot delete Notification. It has related accounts. Delete those accounts first");
        //        //}
        //        _uow.NotificationRepository.Delete(id);
        //        _uow.Save();
        //        baseResponse.statusCode = StatusCodes.Status200OK;
        //        baseResponse.done = true;
        //        return Ok(baseResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Something went wrong inside DeleteNotification action: {ex.Message}");
        //        baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
        //        baseResponse.done = false;
        //        baseResponse.errorMessage = $"Exception :{ex.Message}";
        //        return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
        //    }
        //}

    }
}
