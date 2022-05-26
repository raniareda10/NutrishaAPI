using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.EntitiesV1.Blogs;
using DL.EntitiesV1.Blogs.Polls;
using DL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NutrishaAPI.Controllers.V1.Bases;

namespace NutrishaAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v1/{controller}")]
    public class TestController : BaseMobileController
    {
        private readonly AppDBContext _dbContext;
        private readonly BlogTimelineService _timelineService;

        public TestController(AppDBContext DbContext, BlogTimelineService TimelineService)
        {
            _dbContext = DbContext;
            _timelineService = TimelineService;
        }
        
        [HttpGet("Seed")]
        [AllowAnonymous]
        public async Task<IActionResult> Seed()
        {
            await _dbContext.Blogs.AddRangeAsync(Enumerable.Range(0, 10).Select(r => GenerateBlog(r)));
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        private Blog GenerateBlog(long id)
        {
            var blogs = new Blog()
            {
                Created = DateTime.UtcNow,
                Subject = "Subject " + id,
                OwnerId = 1
            };
                
            if (id % 2 == 0)
            {
                var i = (int) id;
                blogs.TagId = 1;
                blogs.EntityType = EntityType.Article;
                blogs.Totals = new Dictionary<string, int>()
                {
                    {"likes", i},
                    {"comments", i},
                    {"dislikes", i}
                };
                blogs.Article = new Article()
                {
                    
                    Description = "Description " + id
                };
            }
            else
            {
                var lId = (int) id;
                blogs.TagId = 2;
                blogs.EntityType = EntityType.Poll;
                blogs.Poll = new Poll()
                {
                    Questions = Enumerable.Range(lId * 2, lId * 4).Select(i => new PollQuestion()
                    {
                        Content = "PollQuestion" + i,
                        Created = DateTime.Now,
                    }).ToList()
                };
            }

            return blogs;

        }
    }
}