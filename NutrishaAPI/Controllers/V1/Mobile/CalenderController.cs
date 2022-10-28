using System;
using System.Threading.Tasks;
using DL.Repositories.Dashboard;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    public class CalenderController : BaseMobileController
    {
        private readonly CalenderService _calenderService;

        public CalenderController(CalenderService calenderService)
        {
            _calenderService = calenderService;
        }

        [HttpGet("GetDays")]
        public async Task<IActionResult> GetDaysAsync(DateTime startDay, DateTime endDay, bool isSubscribed)
        {
            return ItemResult(await _calenderService.GetBusyDaysAsync(startDay, endDay, isSubscribed));
        }

        [HttpGet("GetDayDetails")]
        public async Task<IActionResult> GetDayInfoAsync(DateTime day, bool isSubscribed)
        {
            return ItemResult(await _calenderService.GetDayDetailsAsync(day, isSubscribed));
        }
        
        
        [HttpGet("GetDashboardDetails")]
        public async Task<IActionResult> GetDashboardDetailsAsync(DateTime day, bool isSubscribed)
        {
            return ItemResult(await _calenderService.GetDashboardDetailsAsync(day, isSubscribed));
        }
    }
}