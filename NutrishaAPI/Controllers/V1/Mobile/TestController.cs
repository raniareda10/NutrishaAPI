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
using DL.Services.Blogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NutrishaAPI.Attributes;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TestController : BaseMobileController
    {
        private readonly AppDBContext _dbContext;
        private readonly BlogService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private Random _random = new Random();
        public TestController(
            AppDBContext DbContext,
            BlogService Service,
            IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = DbContext;
            _service = Service;
            _webHostEnvironment = webHostEnvironment;
        }

        [SecureByCode]
        [AllowAnonymous]
        [HttpGet("Seed")]
        public async Task<IActionResult> Seed()
        {
            if (_webHostEnvironment.EnvironmentName.Contains("prod"))
            {
                return Ok();
            }

            var blogs = Enumerable.Range(0, 10).Select(r => GenerateBlog(r)).ToList();
            blogs.AddRange(
                Enumerable.Range(0, 5).Select(id => new Blog()
                {
                    Created = DateTime.UtcNow.AddDays(_random.Next(-10, 10)),
                    Subject = "Blog Video " + id,
                    OwnerId = 1,
                    TagId = _random.Next(1, 3),
                    EntityType = EntityType.BlogVideo,
                    Totals = new Dictionary<string, int>()
                    {
                        {
                            TotalKeys.Views, 0
                        }
                    },
                    Media = new List<MediaFile>()
                    {
                        new MediaFile()
                        {
                            Url = "https://www.youtube.com/watch?v=zSH15dIl7D0",
                            MediaType = MediaType.Youtube,
                        }
                    }
                })
            );
            await _dbContext.Blogs.AddRangeAsync(blogs);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        private Blog GenerateBlog(long id)
        {
            var blogs = new Blog()
            {
                Created = DateTime.UtcNow.AddDays(_random.Next(-10, 10)),
                Subject = "Subject " + id,
                OwnerId = 1,
                TagId = _random.Next(1, 3),
            };
            
            var random = new Random();
            if (id % 2 == 0)
            {
                var i = (int) id;
                blogs.Media = new List<MediaFile>()
                {
                    new MediaFile()
                    {
                        Url =
                            "https://s.yimg.com/uu/api/res/1.2/ekskmjxgAnWNr1JBsx8JYQ--~B/Zmk9ZmlsbDtoPTQyMjt3PTY3NTthcHBpZD15dGFjaHlvbg--/https://s.yimg.com/os/creatr-uploaded-images/2022-02/cd55c580-86d1-11ec-89b7-2a64fca23029.cf.jpg",
                        MediaType = MediaType.Image,
                        Flags = new HashSet<string>() {MediaFlags.CoverImage}
                    },
                    new MediaFile()
                    {
                        Url = "https://www.youtube.com/watch?v=zSH15dIl7D0",
                        MediaType = MediaType.Youtube,
                    }
                };
                blogs.EntityType = EntityType.Article;
                blogs.Totals = new Dictionary<string, int>()
                {
                    {TotalKeys.Likes, 0},
                    {TotalKeys.Comments, 0},
                };
                blogs.Article = new Article()
                {
                    // Description = $@"Lorem ipsum, dolor sit amet <a href='www.facebook.com'>Facebook</a> consectetur adipisicing elit. Tempore illo
                    // dolore voluptatem quod laborum nemo ullam officia dolores. Maiores iusto
                    // cupiditate repellat harum eaque temporibus fuga quasi fugit minus?
                    // Molestiae. "
                };
            }
            else
            {
                var lId = (int) id;
                blogs.EntityType = EntityType.Poll;
                blogs.Poll = new Poll()
                {
                    Questions = Enumerable.Range(2, 5).Select(i => new PollQuestion()
                    {
                        Content = "PollQuestion" + i,
                        Created = DateTime.UtcNow,
                    }).ToList()
                };
            }

            return blogs;
        }
    }
}