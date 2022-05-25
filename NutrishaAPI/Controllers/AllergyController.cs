using AutoMapper;
using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using DL.DTOs.AllergyDTO;
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
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net;
using NutrishaAPI.Extensions;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    //[ClaimRequirement(ClaimTypes.Role, RoleConstant.Admin)]
   // [AllowAnonymous]
    public class AllergyController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;

        public AllergyController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }



        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(MAllergy), StatusCodes.Status200OK)]
        public IActionResult GetAllAllergy([FromQuery] AllergyQueryPramter AllergyQueryPramter)
        {
            try
            {
                var Allergy = _uow.AllergyRepository.GetAllAllergy(AllergyQueryPramter);
                baseResponse.data = Allergy;
                baseResponse.total_rows = Allergy.Count();
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;


                var metadata = new
                {
                    Allergy.TotalCount,
                    Allergy.PageSize,
                    Allergy.CurrentPage,
                    Allergy.TotalPages,
                    Allergy.HasNext,
                    Allergy.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                _logger.LogInfo($"Returned {Allergy.TotalCount} Allergy from database.");

                return Ok(baseResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllAllergys action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpGet("{id}", Name = "AllergyById")]
        [ProducesResponseType(typeof(MAllergy), StatusCodes.Status200OK)]
        public IActionResult GetAllAllergyId(int id)
        {
            try
            {
                var Allergy = _uow.AllergyRepository.GetById(id);
                if (Allergy == null)
                {
                    _logger.LogError($"Allergy with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Not Found";
                    return NotFound(baseResponse);
                }
                else
                {
                    _logger.LogInfo($"Returned Allergy with id: {id}");
                    //var AllergyResult = _mapper.Map<AllergyDTO>(Allergy);
                    baseResponse.data = Allergy;
                    baseResponse.total_rows = 1;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllergyById action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



 

  

    }
}
