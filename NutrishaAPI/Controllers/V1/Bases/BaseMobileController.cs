using System.Collections.Generic;
using DL.CommonModels.Paging;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Responses;

namespace NutrishaAPI.Controllers.V1.Bases
{
    [ApiController]
    [Route("api/v1/{controller}")]
    public class BaseMobileController : ControllerBase
    {
        protected IActionResult PagedResult<T>(PagedResult<T> pagedResult)
        {
            return Ok(new PagedResponse<IList<T>>()
            {
                Data = pagedResult.Data,
                TotalRows = pagedResult.TotalRows
            });
        }
    }
}