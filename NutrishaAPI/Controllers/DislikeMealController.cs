using AutoMapper;
using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using DL.DTOs.DislikeMealDTO;
using DL.DTOs.DislikeMealDTO;
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
    public class DislikeMealController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;

        public DislikeMealController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }



        
        [HttpGet]
        [ProducesResponseType(typeof(MDislikeMeal), StatusCodes.Status200OK)]
        public IActionResult GetAllDislikeMeal([FromQuery] DislikeMealQueryPramter DislikeMealQueryPramter)
        {
            try
            {
                var DislikeMeal = _uow.DislikeMealRepository.GetAllDislikeMeal(DislikeMealQueryPramter);
                baseResponse.data = DislikeMeal;
                baseResponse.total_rows = DislikeMeal.Count();
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;


                var metadata = new
                {
                    DislikeMeal.TotalCount,
                    DislikeMeal.PageSize,
                    DislikeMeal.CurrentPage,
                    DislikeMeal.TotalPages,
                    DislikeMeal.HasNext,
                    DislikeMeal.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                _logger.LogInfo($"Returned {DislikeMeal.TotalCount} DislikeMeal from database.");

                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllDislikeMeals action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpGet("{id}", Name = "DislikeMealById")]
        [ProducesResponseType(typeof(MDislikeMeal), StatusCodes.Status200OK)]
        public IActionResult GetAllDislikeMealId(int id)
        {
            try
            {
                var DislikeMeal = _uow.DislikeMealRepository.GetById(id);
                if (DislikeMeal == null)
                {
                    _logger.LogError($"DislikeMeal with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Not Found";
                    return NotFound(baseResponse);
                }
                else
                {
                    _logger.LogInfo($"Returned DislikeMeal with id: {id}");
                 //   var DislikeMealResult = _mapper.Map<DislikeMealDTO>(DislikeMeal);
                    baseResponse.data = DislikeMeal;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetDislikeMealById action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }






        [HttpPost]
        [ProducesResponseType(typeof(DislikeMealCreatDto), StatusCodes.Status201Created)]
        public IActionResult CreateDislikeMeal(DislikeMealCreatDto DislikeMeal)
        {
            try
            {
                if (DislikeMeal == null)
                {
                    _logger.LogError("DislikeMeal object sent from dislikeMeal is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid DislikeMeal object sent from dislikeMeal.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid model object";
                    return NotFound(baseResponse);
                }
                var DislikeMealEntity = _mapper.Map<MDislikeMeal>(DislikeMeal);

                _uow.DislikeMealRepository.Add(DislikeMealEntity);
                _uow.Save();
                var createdDislikeMeal = _mapper.Map<DislikeMealCreatDto>(DislikeMealEntity);
                baseResponse.data = createdDislikeMeal;
                baseResponse.statusCode = StatusCodes.Status201Created;
                baseResponse.done = true;
                return CreatedAtRoute("DislikeMealById", new { id = DislikeMealEntity.Id }, baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateDislikeMeal action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MDislikeMeal), StatusCodes.Status200OK)]
        public IActionResult UpdateDislikeMeal(int id, [FromBody] DislikeMealCreatDto DislikeMeal)
        {
            try
            {
                if (DislikeMeal == null)
                {
                    _logger.LogError("DislikeMeal object sent from dislikeMeal is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid DislikeMeal object sent from dislikeMeal.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Invalid";
                    return NotFound(baseResponse);
                }
                var DislikeMealEntity = _uow.DislikeMealRepository.GetById(id);
                if (DislikeMealEntity == null)
                {
                    _logger.LogError($"DislikeMeal with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "object is Null";
                    return NotFound(baseResponse);
                }
                _mapper.Map(DislikeMeal, DislikeMealEntity);
                _uow.DislikeMealRepository.Update(DislikeMealEntity);
                _uow.Save();
                baseResponse.data = DislikeMealEntity;
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateDislikeMeal action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MDislikeMeal), StatusCodes.Status200OK)]
        public IActionResult DeleteDislikeMeal(int id)
        {
            try
            {
                var DislikeMeal = _uow.DislikeMealRepository.GetById(id);
                if (DislikeMeal == null)
                {
                    _logger.LogError($"DislikeMeal with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "hasn't been found in db";
                    return NotFound(baseResponse);
                }

                //if (_repository.DislikeMealClaim.DislikeMealClaimByDislikeMeal(id).Any())
                //{
                //    _logger.LogError($"Cannot delete DislikeMeal with id: {id}. It has related accounts. Delete those accounts first");
                //    return BadRequest("Cannot delete DislikeMeal. It has related accounts. Delete those accounts first");
                //}
                _uow.DislikeMealRepository.Delete(id);
                _uow.Save();
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteDislikeMeal action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }

    }
}
