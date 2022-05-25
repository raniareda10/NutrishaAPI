using System;
using System.Linq;
using System.Net;
using AutoMapper;
using BL.Infrastructure;
using DL.DTOs.MealDTO;
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
    public class MealController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;

        public MealController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }



        
        [HttpGet]
        [ProducesResponseType(typeof(MMeal), StatusCodes.Status200OK)]
        public IActionResult GetAllUserMeal([FromQuery] MealQueryPramter MealQueryPramter)
        {
            try
            {
              var Meal = _uow.MealRepository.GetAllMeal(MealQueryPramter);
                baseResponse.data = Meal;
                baseResponse.total_rows = Meal.Count();
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;


                var metadata = new
                {
                    Meal.TotalCount,
                    Meal.PageSize,
                    Meal.CurrentPage,
                    Meal.TotalPages,
                    Meal.HasNext,
                    Meal.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                _logger.LogInfo($"Returned {Meal.TotalCount} Meal from database.");

                return Ok(baseResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllMeals action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpGet("{id}", Name = "MealById")]
        [ProducesResponseType(typeof(MMeal), StatusCodes.Status200OK)]
        public IActionResult GetAllMealId(int id)
        {
            try
            {
                var Meal = _uow.MealRepository.GetById(id);
                if (Meal == null)
                {
                    _logger.LogError($"Meal with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Not Found";
                    return NotFound(baseResponse);
                }
                else
                {
                    _logger.LogInfo($"Returned Meal with id: {id}");
                 //   var MealResult = _mapper.Map<MealDTO>(Meal);
                    baseResponse.data = Meal;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetMealById action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }






        [HttpPost]
        [ProducesResponseType(typeof(MealCreatDto), StatusCodes.Status201Created)]
        public IActionResult CreateMeal(MealCreatDto Meal)
        {
            try
            {
                if (Meal == null)
                {
                    _logger.LogError("Meal object sent from meal is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Meal object sent from meal.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid model object";
                    return NotFound(baseResponse);
                }
                var MealEntity = _mapper.Map<MMeal>(Meal);

                _uow.MealRepository.Add(MealEntity);
                _uow.Save();
                var createdMeal = _mapper.Map<MealCreatDto>(MealEntity);
                baseResponse.data = createdMeal;
                baseResponse.statusCode = StatusCodes.Status201Created;
                baseResponse.done = true;
                return CreatedAtRoute("MealById", new { id = MealEntity.Id }, baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateMeal action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MMeal), StatusCodes.Status200OK)]
        public IActionResult UpdateMeal(int id, [FromBody] MealCreatDto Meal)
        {
            try
            {
                if (Meal == null)
                {
                    _logger.LogError("Meal object sent from meal is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Meal object sent from meal.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid";
                    return NotFound(baseResponse);
                }
                var MealEntity = _uow.MealRepository.GetById(id);
                if (MealEntity == null)
                {
                    _logger.LogError($"Meal with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                _mapper.Map(Meal, MealEntity);
                _uow.MealRepository.Update(MealEntity);
                _uow.Save();
                baseResponse.data = MealEntity;
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateMeal action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MMeal), StatusCodes.Status200OK)]
        public IActionResult DeleteMeal(int id)
        {
            try
            {
                var Meal = _uow.MealRepository.GetById(id);
                if (Meal == null)
                {
                    _logger.LogError($"Meal with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "hasn't been found in db";
                    return NotFound(baseResponse);
                }

                //if (_repository.MealClaim.MealClaimByMeal(id).Any())
                //{
                //    _logger.LogError($"Cannot delete Meal with id: {id}. It has related accounts. Delete those accounts first");
                //    return BadRequest("Cannot delete Meal. It has related accounts. Delete those accounts first");
                //}
                _uow.MealRepository.Delete(id);
                _uow.Save();
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteMeal action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }

    }
}
