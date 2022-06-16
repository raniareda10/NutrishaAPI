using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.EntitiesV1.Blogs;
using DL.EntitiesV1.Blogs.Articles;
using DL.EntitiesV1.Blogs.Polls;
using DL.EntitiesV1.Media;
using DL.Enums;
using DL.Repositories.Blogs;
using DL.Repositories.Reminders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutrishaAPI.Attributes;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TempEndpointController : BaseMobileController
    {
        private readonly ReminderService _reminderService;
        private readonly AppDBContext _dbContext;

        public TempEndpointController(
            ReminderService reminderService,
            AppDBContext DbContext,
            IWebHostEnvironment webHostEnvironment)
        {
            _reminderService = reminderService;
            _dbContext = DbContext;
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
    }
}