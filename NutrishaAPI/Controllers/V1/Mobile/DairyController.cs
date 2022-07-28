using System.Threading.Tasks;
using DL.DtosV1.Dairies;
using DL.Repositories;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    public class DairyController : BaseMobileController
    {
        private readonly DairyRepository _dairyRepository;

        public DairyController(DairyRepository dairyRepository)
        {
            _dairyRepository = dairyRepository;
        }

        [HttpPost("Post")]
        public async Task<IActionResult> PostAsync([FromBody] CreateDairyDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return InvalidResult("Please Add Name");
            }
            
            return ItemResult(await _dairyRepository.PostAsync(dto));
        }
        
        [HttpGet("GetTodayDairies")]
        public async Task<IActionResult> GetTodayDairiesAsync()
        {
            return ItemResult(await _dairyRepository.GetTodayDairiesAsync());
        }
    }
}