using System;
using System.Linq;
using System.Net;
using AutoMapper;
using BL.Infrastructure;
using DL.DTOs.GoalTypeDTO;
using DL.Entities;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
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
   // [AllowAnonymous]
    public class GoalTypeController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;

        public GoalTypeController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }



        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(MGoalType), StatusCodes.Status200OK)]
        public IActionResult GetAllGoalType([FromQuery] GoalTypeQueryPramter GoalTypeQueryPramter)
        {
            try
            {
                var GoalType = _uow.GoalTypeRepository.GetAllGoalType(GoalTypeQueryPramter);
                baseResponse.data = GoalType;
                baseResponse.total_rows = GoalType.Count();
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;


                var metadata = new
                {
                    GoalType.TotalCount,
                    GoalType.PageSize,
                    GoalType.CurrentPage,
                    GoalType.TotalPages,
                    GoalType.HasNext,
                    GoalType.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                _logger.LogInfo($"Returned {GoalType.TotalCount} GoalType from database.");

                return Ok(baseResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllGoalTypes action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpGet("{id}", Name = "GoalTypeById")]
        [ProducesResponseType(typeof(MGoalType), StatusCodes.Status200OK)]
        public IActionResult GetAllGoalTypeId(int id)
        {
            try
            {
                var GoalType = _uow.GoalTypeRepository.GetById(id);
                if (GoalType == null)
                {
                    _logger.LogError($"GoalType with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Not Found";
                    return NotFound(baseResponse);
                }
                else
                {
                    _logger.LogInfo($"Returned GoalType with id: {id}");
                    //var GoalTypeResult = _mapper.Map<GoalTypeDTO>(GoalType);
                    baseResponse.data = GoalType;
                    baseResponse.total_rows = 1;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetGoalTypeById action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



 

  

    }
}
