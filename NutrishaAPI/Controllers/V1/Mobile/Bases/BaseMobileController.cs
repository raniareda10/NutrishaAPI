using System.Collections.Generic;
using System.Linq;
using System.Net;
using DL.CommonModels.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Attributes;
using NutrishaAPI.Responses;

namespace NutrishaAPI.Controllers.V1.Mobile.Bases
{
    [OnlyMobileUsers]
    [Authorize]
    [Route("mobile/api/v1/[controller]")]
    public class BaseMobileController : SharedApiController
    {
    }
}