using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using DL.CommonModels.Paging;
using NutrishaAPI.Responses;

namespace NutrishaAPI.Controllers.V1
{
    public class SharedApiController : ApiController
    {
        protected IActionResult EmptyResult()
        {
            return Ok(new BaseResponse<object>()
            {
                StatusCode = 200
            });
        }
        protected IActionResult PagedResult<T>(PagedResult<T> pagedResult)
        {
            return Ok(new PagedResponse<IList<T>>()
            {
                Data = pagedResult.Data,
                TotalRows = pagedResult.TotalRows,
                StatusCode = 200
            });
        }
        
        protected IActionResult ListResult<T>(IEnumerable<T> data)
        {
            return Ok(new BaseResponse<IEnumerable<T>>()
            {
                Data = data,
                StatusCode = 200
            });
        }
        
        
        protected IActionResult ItemResult<T>(T data)
        {
            return Ok(new BaseResponse<T>()
            {
                Data = data,
                StatusCode = 200
            });
        }
        
        
        protected IActionResult InvalidResult(string error)
        {
            return BadRequest(new BaseResponse<object>()
            {
                Done = false,
                ErrorMessage = error,
                StatusCode = 400
            });
        }
        protected IActionResult InvalidResult(IEnumerable<string> errors)
        {
            return BadRequest(new BaseResponse<object>()
            {
                Done = false,
                ErrorMessage = errors.First(),
                StatusCode = 400
            });
        }
    }
}