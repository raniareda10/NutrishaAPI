using System;
using System.Linq;
using System.Net;
using AutoMapper;
using BL.Infrastructure;
using DL.DTOs.AttachmentTypeDTO;
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
    public class AttachmentTypeController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;

        public AttachmentTypeController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }



        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(MAttachmentType), StatusCodes.Status200OK)]
        public IActionResult GetAllAttachmentType([FromQuery] AttachmentTypeQueryPramter AttachmentTypeQueryPramter)
        {
            try
            {
                var AttachmentType = _uow.AttachmentTypeRepository.GetAllAttachmentType(AttachmentTypeQueryPramter);
                baseResponse.data = AttachmentType;
                baseResponse.total_rows = AttachmentType.Count();
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;


                var metadata = new
                {
                    AttachmentType.TotalCount,
                    AttachmentType.PageSize,
                    AttachmentType.CurrentPage,
                    AttachmentType.TotalPages,
                    AttachmentType.HasNext,
                    AttachmentType.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                _logger.LogInfo($"Returned {AttachmentType.TotalCount} AttachmentType from database.");

                return Ok(baseResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllAttachmentTypes action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }



        //[HttpGet("{id}", Name = "AttachmentTypeById")]
        //[ProducesResponseType(typeof(MAttachmentType), StatusCodes.Status200OK)]
        //public IActionResult GetAllAttachmentTypeId(int id)
        //{
        //    try
        //    {
        //        var AttachmentType = _uow.AttachmentTypeRepository.GetById(id);
        //        if (AttachmentType == null)
        //        {
        //            _logger.LogError($"AttachmentType with id: {id}, hasn't been found in db.");
        //            baseResponse.done = false;
        //            baseResponse.statusCode = (int)HttpStatusCode.NotFound;
        //            baseResponse.message = "Not Found";
        //            return NotFound(baseResponse);
        //        }
        //        else
        //        {
        //            _logger.LogInfo($"Returned AttachmentType with id: {id}");
        //            //var AttachmentTypeResult = _mapper.Map<AttachmentTypeDTO>(AttachmentType);
        //            baseResponse.data = AttachmentType;
        //            baseResponse.total_rows = 1;
        //            baseResponse.statusCode = (int)HttpStatusCode.OK;
        //            baseResponse.done = true;
        //            return Ok(baseResponse);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Something went wrong inside GetAttachmentTypeById action: {ex.Message}");
        //        baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
        //        baseResponse.done = false;
        //        baseResponse.message = $"Exception :{ex.Message}";
        //        return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
        //    }
        //}



 

  

    }
}
