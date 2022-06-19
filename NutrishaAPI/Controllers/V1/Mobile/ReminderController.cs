using System.Threading.Tasks;
using DL.DtosV1.Allergies;
using DL.DtosV1.Reminders.Mobile;
using DL.Repositories.Reminders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    [Authorize]
    public class ReminderController : BaseMobileController
    {
        private readonly ReminderService _reminderService;

        public ReminderController(ReminderService reminderService)
        {
            _reminderService = reminderService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAllergiesAsync()
        {
            return ListResult(await _reminderService.GetAllAsync());
        }


        [HttpPost("Post")]
        public async Task<IActionResult> PostAsync(PostReminderDto postReminderDto)
        {
            return ItemResult(await _reminderService.PostAsync(postReminderDto));
        }

        [HttpPut("Put")]
        public async Task<IActionResult> PutAsync(PutReminderDto putReminderDto)
        {
            await _reminderService.PutAsync(putReminderDto);
            return EmptyResult();
        }

        [HttpPut("TurnOn")]
        public async Task<IActionResult> PutAsync([FromBody] TurnReminderOnDto reminderOnDto)
        {
            await _reminderService.TurnOnAsync(reminderOnDto);
            return EmptyResult();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteReminderAsync([FromQuery] long id)
        {
            await _reminderService.DeleteReminderAsync(id);
            return EmptyResult();
        }
    }
}