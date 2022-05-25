using System;
using System.Linq;
using System.Net;
using AutoMapper;
using BL.Infrastructure;
using DL.DTOs.MealStepsDTO;
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
    public class MealStepsController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;

        public MealStepsController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }



        
        [HttpGet]
        [ProducesResponseType(typeof(MMealSteps), StatusCodes.Status200OK)]
        public IActionResult GetAllMealSteps([FromQuery] MealStepsQueryPramter MealStepsQueryPramter)
        {
            try
            {
                var MealSteps = _uow.MealStepsRepository.GetAllMealSteps(MealStepsQueryPramter);
                baseResponse.data = MealSteps;
                baseResponse.total_rows = MealSteps.Count();
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;


                var metadata = new
                {
                    MealSteps.TotalCount,
                    MealSteps.PageSize,
                    MealSteps.CurrentPage,
                    MealSteps.TotalPages,
                    MealSteps.HasNext,
                    MealSteps.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                _logger.LogInfo($"Returned {MealSteps.TotalCount} MealSteps from database.");

                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllMealStepss action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpGet("{id}", Name = "MealStepsById")]
        [ProducesResponseType(typeof(MMealSteps), StatusCodes.Status200OK)]
        public IActionResult GetAllMealStepsId(int id)
        {
            try
            {
                var MealSteps = _uow.MealStepsRepository.GetById(id);
                if (MealSteps == null)
                {
                    _logger.LogError($"MealSteps with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Not Found";
                    return NotFound(baseResponse);
                }
                else
                {
                    _logger.LogInfo($"Returned MealSteps with id: {id}");
                 //   var MealStepsResult = _mapper.Map<MealStepsDTO>(MealSteps);
                    baseResponse.data = MealSteps;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetMealStepsById action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }






        [HttpPost]
        [ProducesResponseType(typeof(MealStepsCreatDto), StatusCodes.Status201Created)]
        public IActionResult CreateMealSteps(MealStepsCreatDto MealSteps)
        {
            try
            {
                if (MealSteps == null)
                {
                    _logger.LogError("MealSteps object sent from mealSteps is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid MealSteps object sent from mealSteps.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid model object";
                    return NotFound(baseResponse);
                }
                var MealStepsEntity = _mapper.Map<MMealSteps>(MealSteps);

                _uow.MealStepsRepository.Add(MealStepsEntity);
                _uow.Save();
                var createdMealSteps = _mapper.Map<MealStepsCreatDto>(MealStepsEntity);
                baseResponse.data = createdMealSteps;
                baseResponse.statusCode = StatusCodes.Status201Created;
                baseResponse.done = true;
                return CreatedAtRoute("MealStepsById", new { id = MealStepsEntity.Id }, baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateMealSteps action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MMealSteps), StatusCodes.Status200OK)]
        public IActionResult UpdateMealSteps(int id, [FromBody] MealStepsCreatDto MealSteps)
        {
            try
            {
                if (MealSteps == null)
                {
                    _logger.LogError("MealSteps object sent from mealSteps is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid MealSteps object sent from mealSteps.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid";
                    return NotFound(baseResponse);
                }
                var MealStepsEntity = _uow.MealStepsRepository.GetById(id);
                if (MealStepsEntity == null)
                {
                    _logger.LogError($"MealSteps with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                _mapper.Map(MealSteps, MealStepsEntity);
                _uow.MealStepsRepository.Update(MealStepsEntity);
                _uow.Save();
                baseResponse.data = MealStepsEntity;
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateMealSteps action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MMealSteps), StatusCodes.Status200OK)]
        public IActionResult DeleteMealSteps(int id)
        {
            try
            {
                var MealSteps = _uow.MealStepsRepository.GetById(id);
                if (MealSteps == null)
                {
                    _logger.LogError($"MealSteps with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "hasn't been found in db";
                    return NotFound(baseResponse);
                }

                //if (_repository.MealStepsClaim.MealStepsClaimByMealSteps(id).Any())
                //{
                //    _logger.LogError($"Cannot delete MealSteps with id: {id}. It has related accounts. Delete those accounts first");
                //    return BadRequest("Cannot delete MealSteps. It has related accounts. Delete those accounts first");
                //}
                _uow.MealStepsRepository.Delete(id);
                _uow.Save();
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteMealSteps action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }

    }
}
