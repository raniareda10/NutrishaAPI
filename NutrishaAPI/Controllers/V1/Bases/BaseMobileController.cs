using System.Collections.Generic;
using System.Linq;
using DL.CommonModels.Paging;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Responses;

namespace NutrishaAPI.Controllers.V1.Bases
{
    [ApiController]
    [Route("mobile/api/v1/{controller}")]
    public class BaseMobileController : ControllerBase
    {
        protected IActionResult EmptyResult()
        {
            return Ok(new BaseResponse<object>());
        }
        protected IActionResult PagedResult<T>(PagedResult<T> pagedResult)
        {
            return Ok(new PagedResponse<IList<T>>()
            {
                Data = pagedResult.Data,
                TotalRows = pagedResult.TotalRows,
            });
        }
        
        protected IActionResult ListResult<T>(IEnumerable<T> data)
        {
            return Ok(new BaseResponse<IEnumerable<T>>()
            {
                Data = data,
            });
        }
        
        
        protected IActionResult ObjectResult<T>(T data)
        {
            return Ok(new BaseResponse<T>()
            {
                Data = data,
            });
        }
        
        
        protected IActionResult InvalidResult(string error)
        {
            return BadRequest(new BaseResponse<object>()
            {
                Done = false,
                ErrorMessage = error
            });
        }
        protected IActionResult InvalidResult(IEnumerable<string> errors)
        {
            return BadRequest(new BaseResponse<object>()
            {
                Done = false,
                ErrorMessage = errors.First()
            });
        }
    }
}