using System.Threading.Tasks;
using DL.DtosV1.ContactSupport;
using DL.Repositories.ContactSupport;
using Microsoft.AspNetCore.DataProtection.Internal;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    public class ContactSupportController : BaseMobileController
    {
        private readonly ContactSupportService _contactSupportService;

        public ContactSupportController(ContactSupportService contactSupportService)
        {
            _contactSupportService = contactSupportService;
        }
        
        [HttpPost("Post")]
        public async Task<IActionResult> PostAsync(CreateContactSupportDto model)
        {
            var result = await _contactSupportService.PostAsync(model);
            return ItemResult(result);
        }
        
        [HttpGet("GetAllTypes")]
        public async Task<IActionResult> GetAllTypesAsync()
        {
            return ListResult(await _contactSupportService.GetAllTypes());
        }
    }
}