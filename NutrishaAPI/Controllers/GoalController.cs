using AutoMapper;
using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using DL.DTOs.GoalDTO;
using DL.DTOs.UserGoalDTO;
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
    public class GoalController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;

        public GoalController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }



        
        [HttpGet]
        [ProducesResponseType(typeof(AllGoalDto), StatusCodes.Status200OK)]
        public IActionResult GetAllUserGoal([FromQuery] UserGoalQueryPramter GoalQueryPramter)
        {
            try
            {
                var lstGoal = _uow.UserGoalRepository.GetAllUserGoal(GoalQueryPramter);
                List<AllGoalDto> AllGoal = new List<AllGoalDto>();
                foreach (var item in lstGoal)
                {
      
                    var goal = _mapper.Map<DL.DTOs.GoalDTO.IncludeGoalDto>(item.Goal);
                    //var user = _mapper.Map<DL.DTOs.UserDTO.IncludeUserDto>(item.User);
                    AllGoalDto orderDTO = new AllGoalDto()
                    {
                        UserId = item.UserId,
                        GoalId = item.GoalId,
                        GoalTypeId = goal.GoalTypeId,
                        FrequencyId = goal.FrequencyId,
                        Title = goal.Title,

                    
                    };

                    AllGoal.Add(orderDTO);
                }
                baseResponse.data = AllGoal;
                baseResponse.total_rows = AllGoal.Count();
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;
                return Ok(baseResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllGoals action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpGet("{id}", Name = "GoalById")]
        [ProducesResponseType(typeof(MGoal), StatusCodes.Status200OK)]
        public IActionResult GetAllGoalId(int id)
        {
            try
            {
                var Goal = _uow.GoalRepository.GetById(id);
                if (Goal == null)
                {
                    _logger.LogError($"Goal with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Not Found";
                    return NotFound(baseResponse);
                }
                else
                {
                    _logger.LogInfo($"Returned Goal with id: {id}");
                 //   var GoalResult = _mapper.Map<GoalDTO>(Goal);
                    baseResponse.data = Goal;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetGoalById action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }






        [HttpPost]
        [ProducesResponseType(typeof(GoalCreatDto), StatusCodes.Status201Created)]
        public IActionResult CreateGoal(GoalCreatDto Goal)
        {
            try
            {
                if (Goal == null)
                {
                    _logger.LogError("Goal object sent from goal is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Goal object sent from goal.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid model object";
                    return NotFound(baseResponse);
                }
                var GoalEntity = _mapper.Map<MGoal>(Goal);

                _uow.GoalRepository.Add(GoalEntity);
                _uow.Save();
                var createdGoal = _mapper.Map<GoalCreatDto>(GoalEntity);
                baseResponse.data = createdGoal;
                baseResponse.statusCode = StatusCodes.Status201Created;
                baseResponse.done = true;
                return CreatedAtRoute("GoalById", new { id = GoalEntity.Id }, baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateGoal action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MGoal), StatusCodes.Status200OK)]
        public IActionResult UpdateGoal(int id, [FromBody] GoalCreatDto Goal)
        {
            try
            {
                if (Goal == null)
                {
                    _logger.LogError("Goal object sent from goal is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Goal object sent from goal.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid";
                    return NotFound(baseResponse);
                }
                var GoalEntity = _uow.GoalRepository.GetById(id);
                if (GoalEntity == null)
                {
                    _logger.LogError($"Goal with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                _mapper.Map(Goal, GoalEntity);
                _uow.GoalRepository.Update(GoalEntity);
                _uow.Save();
                baseResponse.data = GoalEntity;
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateGoal action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MGoal), StatusCodes.Status200OK)]
        public IActionResult DeleteGoal(int id)
        {
            try
            {
                var Goal = _uow.GoalRepository.GetById(id);
                if (Goal == null)
                {
                    _logger.LogError($"Goal with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "hasn't been found in db";
                    return NotFound(baseResponse);
                }

                //if (_repository.GoalClaim.GoalClaimByGoal(id).Any())
                //{
                //    _logger.LogError($"Cannot delete Goal with id: {id}. It has related accounts. Delete those accounts first");
                //    return BadRequest("Cannot delete Goal. It has related accounts. Delete those accounts first");
                //}
                _uow.GoalRepository.Delete(id);
                _uow.Save();
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteGoal action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }

    }
}
