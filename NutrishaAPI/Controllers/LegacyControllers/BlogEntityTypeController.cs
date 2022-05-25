using System;
using System.Linq;
using System.Net;
using AutoMapper;
using BL.Infrastructure;
using DL.DTOs.MediaTypeDTO;
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
    public class BlogEntityTypeController : Controller
    {
        private readonly IUnitOfWork _uow; 
        private readonly IHostingEnvironment _hostingEnvironment; 
        private readonly IMapper _mapper;
        private ILoggerManager _logger;
        private readonly BaseResponseHelper baseResponse;

        public BlogEntityTypeController(IUnitOfWork uow ,IHostingEnvironment hostingEnvironment, IMapper mapper, ILoggerManager logger)
        {
            _uow = uow; 
            _hostingEnvironment = _hostingEnvironment;
            _mapper = mapper;
            _logger = logger;
            baseResponse = new BaseResponseHelper();
        }



        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(MMediaType), StatusCodes.Status200OK)]
        public IActionResult GetAllMediaType([FromQuery] MediaTypeQueryPramter MediaTypeQueryPramter)
        {
            try
            {
                var MediaType = _uow.MediaTypeRepository.GetAllMediaType(MediaTypeQueryPramter);
                baseResponse.data = MediaType;
                baseResponse.total_rows = MediaType.Count();
                baseResponse.statusCode = (int)HttpStatusCode.OK;
                baseResponse.done = true;


                var metadata = new
                {
                    MediaType.TotalCount,
                    MediaType.PageSize,
                    MediaType.CurrentPage,
                    MediaType.TotalPages,
                    MediaType.HasNext,
                    MediaType.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                _logger.LogInfo($"Returned {MediaType.TotalCount} MediaType from database.");

                return Ok(baseResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllMediaTypes action: {ex.Message}");
                baseResponse.statusCode = (int)HttpStatusCode.InternalServerError;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";
                return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
            }
        }

  

    }
}
