using AutoMapper;
using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using DL.DTOs.UserMealDTO;
using DL.DTOs.UserMealDTO;
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
    public class UserMealController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;

        public UserMealController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }



        
        [HttpGet]
        [ProducesResponseType(typeof(MUserMeal), StatusCodes.Status200OK)]
        public IActionResult GetAllUserMeal([FromQuery] UserMealQueryPramter UserMealQueryPramter)
        {
            try
            {
                var UserMeal = _uow.UserMealRepository.GetAllUserMeal(UserMealQueryPramter);
                baseResponse.data = UserMeal;
                baseResponse.total_rows = UserMeal.Count();
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;


                var metadata = new
                {
                    UserMeal.TotalCount,
                    UserMeal.PageSize,
                    UserMeal.CurrentPage,
                    UserMeal.TotalPages,
                    UserMeal.HasNext,
                    UserMeal.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                _logger.LogInfo($"Returned {UserMeal.TotalCount} UserMeal from database.");

                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllUserMeals action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpGet("{id}", Name = "UserMealById")]
        [ProducesResponseType(typeof(MUserMeal), StatusCodes.Status200OK)]
        public IActionResult GetAllUserMealId(int id)
        {
            try
            {
                var UserMeal = _uow.UserMealRepository.GetById(id);
                if (UserMeal == null)
                {
                    _logger.LogError($"UserMeal with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Not Found";
                    return NotFound(baseResponse);
                }
                else
                {
                    _logger.LogInfo($"Returned UserMeal with id: {id}");
                 //   var UserMealResult = _mapper.Map<UserMealDTO>(UserMeal);
                    baseResponse.data = UserMeal;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetUserMealById action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }






        [HttpPost]
        [ProducesResponseType(typeof(UserMealCreatDto), StatusCodes.Status201Created)]
        public IActionResult CreateUserMeal(UserMealCreatDto UserMeal)
        {
            try
            {
                if (UserMeal == null)
                {
                    _logger.LogError("UserMeal object sent from userMeal is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid UserMeal object sent from userMeal.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid model object";
                    return NotFound(baseResponse);
                }
                var UserMealEntity = _mapper.Map<MUserMeal>(UserMeal);

                _uow.UserMealRepository.Add(UserMealEntity);
                _uow.Save();
                var createdUserMeal = _mapper.Map<UserMealCreatDto>(UserMealEntity);
                baseResponse.data = createdUserMeal;
                baseResponse.statusCode = StatusCodes.Status201Created;
                baseResponse.done = true;
                return CreatedAtRoute("UserMealById", new { id = UserMealEntity.Id }, baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateUserMeal action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MUserMeal), StatusCodes.Status200OK)]
        public IActionResult UpdateUserMeal(int id, [FromBody] UserMealCreatDto UserMeal)
        {
            try
            {
                if (UserMeal == null)
                {
                    _logger.LogError("UserMeal object sent from userMeal is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid UserMeal object sent from userMeal.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid";
                    return NotFound(baseResponse);
                }
                var UserMealEntity = _uow.UserMealRepository.GetById(id);
                if (UserMealEntity == null)
                {
                    _logger.LogError($"UserMeal with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                _mapper.Map(UserMeal, UserMealEntity);
                _uow.UserMealRepository.Update(UserMealEntity);
                _uow.Save();
                baseResponse.data = UserMealEntity;
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateUserMeal action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MUserMeal), StatusCodes.Status200OK)]
        public IActionResult DeleteUserMeal(int id)
        {
            try
            {
                var UserMeal = _uow.UserMealRepository.GetById(id);
                if (UserMeal == null)
                {
                    _logger.LogError($"UserMeal with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "hasn't been found in db";
                    return NotFound(baseResponse);
                }

                //if (_repository.UserMealClaim.UserMealClaimByUserMeal(id).Any())
                //{
                //    _logger.LogError($"Cannot delete UserMeal with id: {id}. It has related accounts. Delete those accounts first");
                //    return BadRequest("Cannot delete UserMeal. It has related accounts. Delete those accounts first");
                //}
                _uow.UserMealRepository.Delete(id);
                _uow.Save();
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteUserMeal action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }

    }
}
