using AutoMapper;
using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using DL.DTOs.MealIngredientDTO;
using DL.DTOs.MealIngredientDTO;
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
    public class MealIngredientController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;

        public MealIngredientController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }



        
        [HttpGet]
        [ProducesResponseType(typeof(MMealIngredient), StatusCodes.Status200OK)]
        public IActionResult GetAllMealIngredient([FromQuery] MealIngredientQueryPramter MealIngredientQueryPramter)
        {
            try
            {
                var MealIngredient = _uow.MealIngredientRepository.GetAllMealIngredient(MealIngredientQueryPramter);
                baseResponse.data = MealIngredient;
                baseResponse.total_rows = MealIngredient.Count();
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;


                var metadata = new
                {
                    MealIngredient.TotalCount,
                    MealIngredient.PageSize,
                    MealIngredient.CurrentPage,
                    MealIngredient.TotalPages,
                    MealIngredient.HasNext,
                    MealIngredient.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                _logger.LogInfo($"Returned {MealIngredient.TotalCount} MealIngredient from database.");

                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllMealIngredients action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpGet("{id}", Name = "MealIngredientById")]
        [ProducesResponseType(typeof(MMealIngredient), StatusCodes.Status200OK)]
        public IActionResult GetAllMealIngredientId(int id)
        {
            try
            {
                var MealIngredient = _uow.MealIngredientRepository.GetById(id);
                if (MealIngredient == null)
                {
                    _logger.LogError($"MealIngredient with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Not Found";
                    return NotFound(baseResponse);
                }
                else
                {
                    _logger.LogInfo($"Returned MealIngredient with id: {id}");
                 //   var MealIngredientResult = _mapper.Map<MealIngredientDTO>(MealIngredient);
                    baseResponse.data = MealIngredient;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetMealIngredientById action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }






        [HttpPost]
        [ProducesResponseType(typeof(MealIngredientCreatDto), StatusCodes.Status201Created)]
        public IActionResult CreateMealIngredient(MealIngredientCreatDto MealIngredient)
        {
            try
            {
                if (MealIngredient == null)
                {
                    _logger.LogError("MealIngredient object sent from mealIngredient is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid MealIngredient object sent from mealIngredient.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid model object";
                    return NotFound(baseResponse);
                }
                var MealIngredientEntity = _mapper.Map<MMealIngredient>(MealIngredient);

                _uow.MealIngredientRepository.Add(MealIngredientEntity);
                _uow.Save();
                var createdMealIngredient = _mapper.Map<MealIngredientCreatDto>(MealIngredientEntity);
                baseResponse.data = createdMealIngredient;
                baseResponse.statusCode = StatusCodes.Status201Created;
                baseResponse.done = true;
                return CreatedAtRoute("MealIngredientById", new { id = MealIngredientEntity.Id }, baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateMealIngredient action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MMealIngredient), StatusCodes.Status200OK)]
        public IActionResult UpdateMealIngredient(int id, [FromBody] MealIngredientCreatDto MealIngredient)
        {
            try
            {
                if (MealIngredient == null)
                {
                    _logger.LogError("MealIngredient object sent from mealIngredient is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid MealIngredient object sent from mealIngredient.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid";
                    return NotFound(baseResponse);
                }
                var MealIngredientEntity = _uow.MealIngredientRepository.GetById(id);
                if (MealIngredientEntity == null)
                {
                    _logger.LogError($"MealIngredient with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                _mapper.Map(MealIngredient, MealIngredientEntity);
                _uow.MealIngredientRepository.Update(MealIngredientEntity);
                _uow.Save();
                baseResponse.data = MealIngredientEntity;
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateMealIngredient action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MMealIngredient), StatusCodes.Status200OK)]
        public IActionResult DeleteMealIngredient(int id)
        {
            try
            {
                var MealIngredient = _uow.MealIngredientRepository.GetById(id);
                if (MealIngredient == null)
                {
                    _logger.LogError($"MealIngredient with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "hasn't been found in db";
                    return NotFound(baseResponse);
                }

                //if (_repository.MealIngredientClaim.MealIngredientClaimByMealIngredient(id).Any())
                //{
                //    _logger.LogError($"Cannot delete MealIngredient with id: {id}. It has related accounts. Delete those accounts first");
                //    return BadRequest("Cannot delete MealIngredient. It has related accounts. Delete those accounts first");
                //}
                _uow.MealIngredientRepository.Delete(id);
                _uow.Save();
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteMealIngredient action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }

    }
}
