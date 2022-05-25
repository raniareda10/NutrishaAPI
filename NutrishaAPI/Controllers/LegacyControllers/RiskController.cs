using System;
using System.Linq;
using System.Net;
using AutoMapper;
using BL.Infrastructure;
using DL.DTOs.RiskDTO;
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
    public class RiskController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;

        public RiskController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }



        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(MRisk), StatusCodes.Status200OK)]
        public IActionResult GetAllRisk([FromQuery] RiskQueryPramter RiskQueryPramter)
        {
            try
            {
                var Risk = _uow.RiskRepository.GetAllRisk(RiskQueryPramter);
                baseResponse.data = Risk;
                baseResponse.total_rows = Risk.Count();
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;


                var metadata = new
                {
                    Risk.TotalCount,
                    Risk.PageSize,
                    Risk.CurrentPage,
                    Risk.TotalPages,
                    Risk.HasNext,
                    Risk.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                _logger.LogInfo($"Returned {Risk.TotalCount} Risk from database.");

                return Ok(baseResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllRisks action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpGet("{id}", Name = "RiskById")]
        [ProducesResponseType(typeof(MRisk), StatusCodes.Status200OK)]
        public IActionResult GetAllRiskId(int id)
        {
            try
            {
                var Risk = _uow.RiskRepository.GetById(id);
                if (Risk == null)
                {
                    _logger.LogError($"Risk with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Not Found";
                    return NotFound(baseResponse);
                }
                else
                {
                    _logger.LogInfo($"Returned Risk with id: {id}");
                    //var RiskResult = _mapper.Map<RiskDTO>(Risk);
                    baseResponse.data = Risk;
                    baseResponse.total_rows = 1;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetRiskById action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



 

  

    }
}
