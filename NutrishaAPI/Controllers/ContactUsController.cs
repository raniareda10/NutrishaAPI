using AutoMapper;
using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using DL.DTOs.ContactUsDTO;
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


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    //[ClaimRequirement(ClaimTypes.Role, RoleConstant.Admin)]
    public class ContactUsController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;


        public ContactUsController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }



        
        [HttpGet]
        [ProducesResponseType(typeof(IQueryable<MContactUs>), StatusCodes.Status200OK)]
        public IActionResult GetAllContactUs([FromQuery] ContactUsQueryPramter ContactUsQueryPramter)
        {
            try
            {
                var ContactUs = _uow.ContactUsRepository.GetAllContactUs(ContactUsQueryPramter);



                var metadata = new
                {
                    ContactUs.TotalCount,
                    ContactUs.PageSize,
                    ContactUs.CurrentPage,
                    ContactUs.TotalPages,
                    ContactUs.HasNext,
                    ContactUs.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                _logger.LogInfo($"Returned {ContactUs.TotalCount} ContactUs from database.");

                baseResponse.data = ContactUs;
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllContactUss action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.errorMessage = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpGet("{id}", Name = "ContactUsById")]
        [ProducesResponseType(typeof(IQueryable<MContactUs>), StatusCodes.Status200OK)]
        public IActionResult GetAllContactUsId(int id)
        {
            try
            {
                var ContactUs = _uow.ContactUsRepository.GetById(id);
                if (ContactUs == null)
                {
                    _logger.LogError($"ContactUs with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.errorMessage = "Not Found";
                    return NotFound(baseResponse);
                }
                else
                {
                    _logger.LogInfo($"Returned ContactUs with id: {id}");
                    //var ContactUsResult = _mapper.Map<ContactUsDTO>(ContactUs);
                    baseResponse.data = ContactUs;
                    baseResponse.statusCode = (int)HttpStatusCode.OK;
                    baseResponse.done = true;
                    return Ok(ContactUs);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetContactUsById action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.errorMessage = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



 

  
        [HttpGet, Route("ActivateContactUs")]
        public IActionResult ActivateContactUs(int Id)
        {
            try
            {
                var ContactUs = _uow.ContactUsRepository.GetById(Id);
                ContactUs.IsActive = true;
                _uow.ContactUsRepository.Update(ContactUs);
                _uow.Save();
                return Content("ContactUs Activated");
            }
            catch (Exception ex)
            {

                return Content(ex.ToString());
            }


        }




        [HttpPost]
        [ProducesResponseType(typeof(ContactUsCreatDto), StatusCodes.Status201Created)]
        public IActionResult CreateContactUs([FromForm] ContactUsCreatDto ContactUs)
        {
            try
            {
                if (ContactUs == null)
                {
                    _logger.LogError("ContactUs object sent from contactUs is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.errorMessage = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid ContactUs object sent from contactUs.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.errorMessage = "Invalid model object";
                    return NotFound(baseResponse);
                }
                var ContactUsEntity = _mapper.Map<MContactUs>(ContactUs);
               
                _uow.ContactUsRepository.Add(ContactUsEntity);
                _uow.Save();
                var createdContactUs = _mapper.Map<ContactUsCreatDto>(ContactUsEntity);
                baseResponse.data = createdContactUs;
                baseResponse.statusCode = StatusCodes.Status201Created;
                baseResponse.done = true;
                return CreatedAtRoute("ContactUsById", new { id = ContactUsEntity.Id }, baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateContactUs action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.errorMessage = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IQueryable<MContactUs>), StatusCodes.Status200OK)]
        public IActionResult UpdateContactUs(int id, [FromBody] ContactUsCreatDto ContactUs)
        {
            try
            {
                if (ContactUs == null)
                {
                    _logger.LogError("ContactUs object sent from contactUs is null.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.errorMessage = "object is Null";
                    return NotFound(baseResponse);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid ContactUs object sent from contactUs.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.errorMessage = "Invalid";
                    return NotFound(baseResponse);
                }
                var ContactUsEntity = _uow.ContactUsRepository.GetById(id);
                if (ContactUsEntity == null)
                {
                    _logger.LogError($"ContactUs with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.errorMessage = "object is Null";
                    return NotFound(baseResponse);
                }
                _mapper.Map(ContactUs, ContactUsEntity);
                _uow.ContactUsRepository.Update(ContactUsEntity);
                _uow.Save();
                baseResponse.data = ContactUsEntity;
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateContactUs action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.errorMessage = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(IQueryable<MContactUs>), StatusCodes.Status200OK)]
        public IActionResult DeleteContactUs(int id)
        {
            try
            {
                var ContactUs = _uow.ContactUsRepository.GetById(id);
                if (ContactUs == null)
                {
                    _logger.LogError($"ContactUs with id: {id}, hasn't been found in db.");
                    baseResponse.done = false;
                    baseResponse.statusCode = (int)HttpStatusCode.NotFound;
                    baseResponse.errorMessage = "hasn't been found in db";
                    return NotFound(baseResponse);
                }

                //if (_repository.ContactUsClaim.ContactUsClaimByContactUs(id).Any())
                //{
                //    _logger.LogError($"Cannot delete ContactUs with id: {id}. It has related accounts. Delete those accounts first");
                //    return BadRequest("Cannot delete ContactUs. It has related accounts. Delete those accounts first");
                //}
                _uow.ContactUsRepository.Delete(id);
                _uow.Save();
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteContactUs action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.errorMessage = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }

    }
}
