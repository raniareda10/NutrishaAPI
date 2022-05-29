using System.Threading.Tasks;
using DL.DtosV1.Polls;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Attributes;
using NutrishaAPI.Validations.Polls;
using Org.BouncyCastle.Bcpg;

namespace NutrishaAPI.Controllers.V1.Admin.V1
{
    [OnlyAdmins]
    [Route("Admin/api/v1/[controller]")]
    public abstract class BaseAdminV1Controller : SharedApiController
    {
    }
}