using System.Threading.Tasks;
using DL.DtosV1.Allergies;
using DL.Repositories.Allergy;
using DL.ResultModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    [Authorize]
    public class AllergyController : BaseMobileController
    {
        private readonly AllergyService _allergyService;
        private readonly string _locale;
        public AllergyController(AllergyService allergyService, IHttpContextAccessor httpContextAccessor)
        {
            _allergyService = allergyService;
            _locale = httpContextAccessor.HttpContext.Request.Headers["Accept-Language"];
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAllergiesAsync()
        {
            return ListResult(await _allergyService.GetAllAsync(_locale));
        }
        
        [HttpGet("GetSelectedAllergies")]
        public async Task<IActionResult> GetSelectedAllergiesAsync()
        {
            return ListResult(await _allergyService.GetSelectAllergyNamesAsync(_locale));
        }
        
        
        [HttpPut("Put")]
        public async Task<IActionResult> PutAsync(PutAllergyDto allergyDto)
        {
            await _allergyService.PutAsync(allergyDto);
            return EmptyResult();
        }

        [HttpPost("Post")]
        public async Task<IActionResult> PostAsync([FromBody] PostAllergyDto postAllergyDto)
        {
            if (string.IsNullOrWhiteSpace(postAllergyDto.AllergyName))
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            
            var result = await _allergyService.AddCustomAllergiesAsync(postAllergyDto.AllergyName, postAllergyDto.AllergyNameAr);
            return ItemResult(result);
        }
    }
}