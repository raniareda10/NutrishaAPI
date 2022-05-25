using AutoMapper;
using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using DL.DTOs.FrequencyDTO;
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
    public class FrequencyController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;

        public FrequencyController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }



        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(MFrequency), StatusCodes.Status200OK)]
        public IActionResult GetAllFrequency([FromQuery] FrequencyQueryPramter FrequencyQueryPramter)
        {
            try
            {
                var Frequency = _uow.FrequencyRepository.GetAllFrequency(FrequencyQueryPramter);
                baseResponse.data = Frequency;
                baseResponse.total_rows = Frequency.Count();
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;


                var metadata = new
                {
                    Frequency.TotalCount,
                    Frequency.PageSize,
                    Frequency.CurrentPage,
                    Frequency.TotalPages,
                    Frequency.HasNext,
                    Frequency.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                _logger.LogInfo($"Returned {Frequency.TotalCount} Frequency from database.");

                return Ok(baseResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllFrequencys action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpGet("{id}", Name = "FrequencyById")]
        [ProducesResponseType(typeof(MFrequency), StatusCodes.Status200OK)]
        public IActionResult GetAllFrequencyId(int id)
        {
            try
            {
                var Frequency = _uow.FrequencyRepository.GetById(id);
                if (Frequency == null)
                {
                    _logger.LogError($"Frequency with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.message = "Not Found";
                    return NotFound(baseResponse);
                }
                else
                {
                    _logger.LogInfo($"Returned Frequency with id: {id}");
                    //var FrequencyResult = _mapper.Map<FrequencyDTO>(Frequency);
                    baseResponse.data = Frequency;
                    baseResponse.total_rows = 1;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;
                    baseResponse.done = true;
                    return Ok(baseResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetFrequencyById action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



 

  

    }
}
