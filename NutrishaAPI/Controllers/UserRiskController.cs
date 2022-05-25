using AutoMapper;
using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using DL.DTOs.UserRiskDTO;
using DL.DTOs.UserRiskDTO;
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
    public class UserRiskController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;

        public UserRiskController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }



        
        [HttpGet]
        [ProducesResponseType(typeof(MUserRisk), StatusCodes.Status200OK)]
        public IActionResult GetAllUserRisk([FromQuery] UserRiskQueryPramter UserRiskQueryPramter)
        {
            try
            {
                var UserRisk = _uow.UserRiskRepository.GetAllUserRisk(UserRiskQueryPramter);
                baseResponse.data = UserRisk;
                baseResponse.total_rows = UserRisk.Count();
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;


                var metadata = new
                {
                    UserRisk.TotalCount,
                    UserRisk.PageSize,
                    UserRisk.CurrentPage,
                    UserRisk.TotalPages,
                    UserRisk.HasNext,
                    UserRisk.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                _logger.LogInfo($"Returned {UserRisk.TotalCount} UserRisk from database.");

                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllUserRisks action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpGet("{id}", Name = "UserRiskById")]
        [ProducesResponseType(typeof(MUserRisk), StatusCodes.Status200OK)]
        public IActionResult GetAllUserRiskId(int id)
        {
            try
            {
                var UserRisk = _uow.UserRiskRepository.GetById(id);
                if (UserRisk == null)
                {
                    _logger.LogError($"UserRisk with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Not Found";
                    return NotFound(baseResponse);
                }
                else
                {
                    _logger.LogInfo($"Returned UserRisk with id: {id}");
                 //   var UserRiskResult = _mapper.Map<UserRiskDTO>(UserRisk);
                    baseResponse.data = UserRisk;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetUserRiskById action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }






        [HttpPost]
        [ProducesResponseType(typeof(UserRiskCreatDto), StatusCodes.Status201Created)]
        public IActionResult CreateUserRisk(UserRiskCreatDto UserRisk)
        {
            try
            {
                if (UserRisk == null)
                {
                    _logger.LogError("UserRisk object sent from userRisk is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid UserRisk object sent from userRisk.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid model object";
                    return NotFound(baseResponse);
                }
                var UserRiskEntity = _mapper.Map<MUserRisk>(UserRisk);

                _uow.UserRiskRepository.Add(UserRiskEntity);
                _uow.Save();
                var createdUserRisk = _mapper.Map<UserRiskCreatDto>(UserRiskEntity);
                baseResponse.data = createdUserRisk;
                baseResponse.statusCode = StatusCodes.Status201Created;
                baseResponse.done = true;
                return CreatedAtRoute("UserRiskById", new { id = UserRiskEntity.Id }, baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateUserRisk action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MUserRisk), StatusCodes.Status200OK)]
        public IActionResult UpdateUserRisk(int id, [FromBody] UserRiskCreatDto UserRisk)
        {
            try
            {
                if (UserRisk == null)
                {
                    _logger.LogError("UserRisk object sent from userRisk is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid UserRisk object sent from userRisk.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid";
                    return NotFound(baseResponse);
                }
                var UserRiskEntity = _uow.UserRiskRepository.GetById(id);
                if (UserRiskEntity == null)
                {
                    _logger.LogError($"UserRisk with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                _mapper.Map(UserRisk, UserRiskEntity);
                _uow.UserRiskRepository.Update(UserRiskEntity);
                _uow.Save();
                baseResponse.data = UserRiskEntity;
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateUserRisk action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MUserRisk), StatusCodes.Status200OK)]
        public IActionResult DeleteUserRisk(int id)
        {
            try
            {
                var UserRisk = _uow.UserRiskRepository.GetById(id);
                if (UserRisk == null)
                {
                    _logger.LogError($"UserRisk with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "hasn't been found in db";
                    return NotFound(baseResponse);
                }

                //if (_repository.UserRiskClaim.UserRiskClaimByUserRisk(id).Any())
                //{
                //    _logger.LogError($"Cannot delete UserRisk with id: {id}. It has related accounts. Delete those accounts first");
                //    return BadRequest("Cannot delete UserRisk. It has related accounts. Delete those accounts first");
                //}
                _uow.UserRiskRepository.Delete(id);
                _uow.Save();
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteUserRisk action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }

    }
}
