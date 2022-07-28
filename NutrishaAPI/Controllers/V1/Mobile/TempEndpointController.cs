using System;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.Repositories.Allergy;
using DL.Repositories.Dislikes;
using DL.Repositories.Reminders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NutrishaAPI.Attributes;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/v1/[controller]")]
    public class TempEndpointController : BaseMobileController
    {
        private readonly IConfiguration _configuration;
        private readonly AllergyService _allergyService;
        private readonly ReminderService _reminderService;
        private readonly AppDBContext _dbContext;
        private readonly DislikesMealService _dislikesMealService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TempEndpointController(
            IConfiguration configuration,
            AllergyService allergyService,
            ReminderService reminderService,
            AppDBContext DbContext,
            DislikesMealService dislikesMealService,
            IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _allergyService = allergyService;
            _reminderService = reminderService;
            _dbContext = DbContext;
            _dislikesMealService = dislikesMealService;
            _webHostEnvironment = webHostEnvironment;
        }

        [SecureByCode]
        [AllowAnonymous]
        [HttpPost("SyncReminder")]
        public async Task<IActionResult> SyncReminder()
        {
            var userIds = await _dbContext.MUser.Select(u => u.Id).ToListAsync();

            foreach (var id in userIds)
            {
                await _reminderService.CreateDefaultRemindersAsync(id);
            }

            return Ok();
        }

        [SecureByCode]
        [AllowAnonymous]
        [HttpPost("SyncAllergies")]
        public async Task<IActionResult> SyncAllergies()
        {
            var userIds = await _dbContext.MUser.Select(u => u.Id).ToListAsync();

            foreach (var id in userIds)
            {
                await _allergyService.AddDefaultAllergiesToUser(id);
            }

            return Ok();
        }

        [SecureByCode]
        [AllowAnonymous]
        [HttpPost("SyncDislikes")]
        public async Task<IActionResult> SyncDislikes()
        {
            var userIds = await _dbContext.MUser.Select(u => u.Id).ToListAsync();

            foreach (var id in userIds)
            {
                await _dislikesMealService.AddDefaultDislikesAsync(id);
            }

            return Ok();
        }

        [SecureByCode]
        [AllowAnonymous]
        [HttpGet("GetConfiguration")]
        public IActionResult GetConfiguration(string path, string section)
        {
            // TODO: Will be enabled when goes to prod
            // if (_webHostEnvironment.IsProduction())
            // {
            //     return Ok();
            // }

            return !string.IsNullOrWhiteSpace(section)
                ? Ok(_configuration.GetSection(section).AsEnumerable())
                : Ok(_configuration[path]);
        }

        [SecureByCode]
        [AllowAnonymous]
        [HttpGet("GetEnv")]
        public IActionResult GetEnv()
        {
            return Ok(new
            {
                env = Environment.GetEnvironmentVariables(),
                HostingEnv = _webHostEnvironment
            });
        }
    }
}