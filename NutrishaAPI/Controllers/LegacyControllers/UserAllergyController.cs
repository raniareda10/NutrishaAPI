using System;
using System.Linq;
using System.Net;
using AutoMapper;
using BL.Infrastructure;
using DL.DTOs.UserAllergyDTO;
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
    public class UserAllergyController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;

        public UserAllergyController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }



        
        [HttpGet]
        [ProducesResponseType(typeof(MUserAllergy), StatusCodes.Status200OK)]
        public IActionResult GetAllUserAllergy([FromQuery] UserAllergyQueryPramter UserAllergyQueryPramter)
        {
            try
            {
                var UserAllergy = _uow.UserAllergyRepository.GetAllUserAllergy(UserAllergyQueryPramter);
                baseResponse.data = UserAllergy;
                baseResponse.total_rows = UserAllergy.Count();
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;


                var metadata = new
                {
                    UserAllergy.TotalCount,
                    UserAllergy.PageSize,
                    UserAllergy.CurrentPage,
                    UserAllergy.TotalPages,
                    UserAllergy.HasNext,
                    UserAllergy.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                _logger.LogInfo($"Returned {UserAllergy.TotalCount} UserAllergy from database.");

                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllUserAllergys action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpGet("{id}", Name = "UserAllergyById")]
        [ProducesResponseType(typeof(MUserAllergy), StatusCodes.Status200OK)]
        public IActionResult GetAllUserAllergyId(int id)
        {
            try
            {
                var UserAllergy = _uow.UserAllergyRepository.GetById(id);
                if (UserAllergy == null)
                {
                    _logger.LogError($"UserAllergy with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Not Found";
                    return NotFound(baseResponse);
                }
                else
                {
                    _logger.LogInfo($"Returned UserAllergy with id: {id}");
                 //   var UserAllergyResult = _mapper.Map<UserAllergyDTO>(UserAllergy);
                    baseResponse.data = UserAllergy;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetUserAllergyById action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }






        [HttpPost]
        [ProducesResponseType(typeof(UserAllergyCreatDto), StatusCodes.Status201Created)]
        public IActionResult CreateUserAllergy(UserAllergyCreatDto UserAllergy)
        {
            try
            {
                if (UserAllergy == null)
                {
                    _logger.LogError("UserAllergy object sent from userAllergy is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid UserAllergy object sent from userAllergy.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid model object";
                    return NotFound(baseResponse);
                }
                var UserAllergyEntity = _mapper.Map<MUserAllergy>(UserAllergy);

                _uow.UserAllergyRepository.Add(UserAllergyEntity);
                _uow.Save();
                var createdUserAllergy = _mapper.Map<UserAllergyCreatDto>(UserAllergyEntity);
                baseResponse.data = createdUserAllergy;
                baseResponse.statusCode = StatusCodes.Status201Created;
                baseResponse.done = true;
                return CreatedAtRoute("UserAllergyById", new { id = UserAllergyEntity.Id }, baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateUserAllergy action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MUserAllergy), StatusCodes.Status200OK)]
        public IActionResult UpdateUserAllergy(int id, [FromBody] UserAllergyCreatDto UserAllergy)
        {
            try
            {
                if (UserAllergy == null)
                {
                    _logger.LogError("UserAllergy object sent from userAllergy is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid UserAllergy object sent from userAllergy.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid";
                    return NotFound(baseResponse);
                }
                var UserAllergyEntity = _uow.UserAllergyRepository.GetById(id);
                if (UserAllergyEntity == null)
                {
                    _logger.LogError($"UserAllergy with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                _mapper.Map(UserAllergy, UserAllergyEntity);
                _uow.UserAllergyRepository.Update(UserAllergyEntity);
                _uow.Save();
                baseResponse.data = UserAllergyEntity;
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateUserAllergy action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MUserAllergy), StatusCodes.Status200OK)]
        public IActionResult DeleteUserAllergy(int id)
        {
            try
            {
                var UserAllergy = _uow.UserAllergyRepository.GetById(id);
                if (UserAllergy == null)
                {
                    _logger.LogError($"UserAllergy with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "hasn't been found in db";
                    return NotFound(baseResponse);
                }

                //if (_repository.UserAllergyClaim.UserAllergyClaimByUserAllergy(id).Any())
                //{
                //    _logger.LogError($"Cannot delete UserAllergy with id: {id}. It has related accounts. Delete those accounts first");
                //    return BadRequest("Cannot delete UserAllergy. It has related accounts. Delete those accounts first");
                //}
                _uow.UserAllergyRepository.Delete(id);
                _uow.Save();
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteUserAllergy action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }

    }
}
