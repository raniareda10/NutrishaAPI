using System.Threading.Tasks;
using DL.DtosV1.Allergies;
using DL.Services.Allergy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    [Authorize]
    public class AllergyController : BaseMobileController
    {
        private readonly AllergyService _allergyService;

        public AllergyController(AllergyService allergyService)
        {
            _allergyService = allergyService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAllergiesAsync()
        {
            return ListResult(await _allergyService.GetAllAsync());
        }


        [HttpGet("GetSelectedAllergies")]
        public async Task<IActionResult> GetSelectedAllergiesAsync()
        {
            return ListResult(await _allergyService.GetSelectedAllergiesAsync());
        }

        [HttpPut("Put")]
        public async Task<IActionResult> PutAsync(PutAllergyDto allergyDto)
        {
            await _allergyService.PutAsync(allergyDto);
            return EmptyResult();
        }
    }
}