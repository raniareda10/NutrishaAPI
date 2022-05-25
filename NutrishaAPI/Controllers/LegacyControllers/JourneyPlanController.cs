using System;
using System.Linq;
using System.Net;
using AutoMapper;
using BL.Infrastructure;
using DL.DTOs.JourneyPlanDTO;
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
    public class JourneyPlanController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;

        public JourneyPlanController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }



        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(MJourneyPlan), StatusCodes.Status200OK)]
        public IActionResult GetAllJourneyPlan([FromQuery] JourneyPlanQueryPramter JourneyPlanQueryPramter)
        {
            try
            {
                var JourneyPlan = _uow.JourneyPlanRepository.GetAllJourneyPlan(JourneyPlanQueryPramter);
                baseResponse.data = JourneyPlan;
                baseResponse.total_rows = JourneyPlan.Count();
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;


                var metadata = new
                {
                    JourneyPlan.TotalCount,
                    JourneyPlan.PageSize,
                    JourneyPlan.CurrentPage,
                    JourneyPlan.TotalPages,
                    JourneyPlan.HasNext,
                    JourneyPlan.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                _logger.LogInfo($"Returned {JourneyPlan.TotalCount} JourneyPlan from database.");

                return Ok(baseResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllJourneyPlans action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpGet("{id}", Name = "JourneyPlanById")]
        [ProducesResponseType(typeof(MJourneyPlan), StatusCodes.Status200OK)]
        public IActionResult GetAllJourneyPlanId(int id)
        {
            try
            {
                var JourneyPlan = _uow.JourneyPlanRepository.GetById(id);
                if (JourneyPlan == null)
                {
                    _logger.LogError($"JourneyPlan with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Not Found";
                    return NotFound(baseResponse);
                }
                else
                {
                    _logger.LogInfo($"Returned JourneyPlan with id: {id}");
                    //var JourneyPlanResult = _mapper.Map<JourneyPlanDTO>(JourneyPlan);
                    baseResponse.data = JourneyPlan;
                    baseResponse.total_rows = 1;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetJourneyPlanById action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



 

  

    }
}
