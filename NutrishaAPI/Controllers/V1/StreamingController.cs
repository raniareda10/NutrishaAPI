﻿using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NutrishaAPI.Controllers.V1
{
    [AllowAnonymous]
    [Route("Streaming")]
    public class StreamingController : ApiController
    {
        [HttpGet, Route("")]
        public FileResult Get(string filePath)
        {
            var path = Directory.GetCurrentDirectory() + $"/wwwroot/{filePath}";
            return PhysicalFile(path,  "application/octet-stream", true);
        }
    }
}