using System;
using System.Linq;
using System.Net;
using AutoMapper;
using BL.Infrastructure;
using DL.DTOs.UserGoalDTO;
using DL.Entities;
using LoggerService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NutrishaAPI.Extensions;

namespace NutrishaAPI.Controllers.LegacyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    //[ClaimRequirement(ClaimTypes.Role, RoleConstant.Admin)]
    public class UserGoalController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;

        public UserGoalController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }



        
        [HttpGet]
        [ProducesResponseType(typeof(MUserGoal), StatusCodes.Status200OK)]
        public IActionResult GetAllUserGoal([FromQuery] UserGoalQueryPramter UserGoalQueryPramter)
        {
            try
            {
                var UserGoal = _uow.UserGoalRepository.GetAllUserGoal(UserGoalQueryPramter);
                baseResponse.data = UserGoal;
                baseResponse.total_rows = UserGoal.Count();
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;


                var metadata = new
                {
                    UserGoal.TotalCount,
                    UserGoal.PageSize,
                    UserGoal.CurrentPage,
                    UserGoal.TotalPages,
                    UserGoal.HasNext,
                    UserGoal.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                _logger.LogInfo($"Returned {UserGoal.TotalCount} UserGoal from database.");

                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllUserGoals action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpGet("{id}", Name = "UserGoalById")]
        [ProducesResponseType(typeof(MUserGoal), StatusCodes.Status200OK)]
        public IActionResult GetAllUserGoalId(int id)
        {
            try
            {
                var UserGoal = _uow.UserGoalRepository.GetById(id);
                if (UserGoal == null)
                {
                    _logger.LogError($"UserGoal with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Not Found";
                    return NotFound(baseResponse);
                }
                else
                {
                    _logger.LogInfo($"Returned UserGoal with id: {id}");
                 //   var UserGoalResult = _mapper.Map<UserGoalDTO>(UserGoal);
                    baseResponse.data = UserGoal;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetUserGoalById action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }






        [HttpPost]
        [ProducesResponseType(typeof(UserGoalCreatDto), StatusCodes.Status201Created)]
        public IActionResult CreateUserGoal(UserGoalCreatDto UserGoal)
        {
            try
            {
                if (UserGoal == null)
                {
                    _logger.LogError("UserGoal object sent from userGoal is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid UserGoal object sent from userGoal.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid model object";
                    return NotFound(baseResponse);
                }
                var UserGoalEntity = _mapper.Map<MUserGoal>(UserGoal);

                _uow.UserGoalRepository.Add(UserGoalEntity);
                _uow.Save();
                var createdUserGoal = _mapper.Map<UserGoalCreatDto>(UserGoalEntity);
                baseResponse.data = createdUserGoal;
                baseResponse.statusCode = StatusCodes.Status201Created;
                baseResponse.done = true;
                return CreatedAtRoute("UserGoalById", new { id = UserGoalEntity.Id }, baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateUserGoal action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MUserGoal), StatusCodes.Status200OK)]
        public IActionResult UpdateUserGoal(int id, [FromBody] UserGoalCreatDto UserGoal)
        {
            try
            {
                if (UserGoal == null)
                {
                    _logger.LogError("UserGoal object sent from userGoal is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid UserGoal object sent from userGoal.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid";
                    return NotFound(baseResponse);
                }
                var UserGoalEntity = _uow.UserGoalRepository.GetById(id);
                if (UserGoalEntity == null)
                {
                    _logger.LogError($"UserGoal with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                _mapper.Map(UserGoal, UserGoalEntity);
                _uow.UserGoalRepository.Update(UserGoalEntity);
                _uow.Save();
                baseResponse.data = UserGoalEntity;
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateUserGoal action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MUserGoal), StatusCodes.Status200OK)]
        public IActionResult DeleteUserGoal(int id)
        {
            try
            {
                var UserGoal = _uow.UserGoalRepository.GetById(id);
                if (UserGoal == null)
                {
                    _logger.LogError($"UserGoal with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "hasn't been found in db";
                    return NotFound(baseResponse);
                }

                //if (_repository.UserGoalClaim.UserGoalClaimByUserGoal(id).Any())
                //{
                //    _logger.LogError($"Cannot delete UserGoal with id: {id}. It has related accounts. Delete those accounts first");
                //    return BadRequest("Cannot delete UserGoal. It has related accounts. Delete those accounts first");
                //}
                _uow.UserGoalRepository.Delete(id);
                _uow.Save();
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteUserGoal action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }

    }
}
