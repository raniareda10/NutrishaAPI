﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DL.EntitiesV1.Media;
using DL.Enums;
using DL.Extensions;
using DL.HelperInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
namespace DL.StorageServices
{
    public class StorageService : IStorageService
    {
        private readonly string _hostDomain;

        public StorageService(IConfiguration configuration)
        {
            _hostDomain = configuration["Domain"];
        }

        public async Task<IList<MediaFile>> UploadAsync(IMedia model,
            // string entityId,
            EntityType entityType)
        {
            var filesCount = model.Files?.Count ?? 0;
            var externalMediaCount = model.ExternalMedia?.Count ?? 0;
            var count = filesCount + externalMediaCount;

            if (count == 0) return null;
            var mediaFiles = new List<MediaFile>(count);

            if (externalMediaCount != 0)
            {
                mediaFiles.AddRange(model.ExternalMedia.Select(ex => new MediaFile()
                {
                    Id = Guid.NewGuid(),
                    Url = ex.Url,
                    Flags = ex.Flags,
                    MediaType = MediaExtensions.ExtractExternalUrlType(ex.Url),
                    Thumbnail = null
                }));
            }

            if (filesCount == 0) return mediaFiles;

            var path = $"{entityType}-{Guid.NewGuid()}";
            var currentDirectory = Directory.GetCurrentDirectory() + "/wwwroot";
            Directory.CreateDirectory($"{currentDirectory}/{path}");
            foreach (var fileModel in model.Files)
            {
                mediaFiles.Add(new MediaFile()
                {
                    Id = Guid.NewGuid(),
                    Url = await UploadFileHelperAsync(fileModel.File, path, currentDirectory),
                    Flags = fileModel.Flags,
                    MediaType = MediaExtensions.GetFileType(fileModel.File),
                    Thumbnail = null
                });
            }

            return mediaFiles;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string path)
        {
            var currentDirectory = Directory.GetCurrentDirectory() + "/wwwroot";
            Directory.CreateDirectory($"{currentDirectory}/{path}");
            return await UploadFileHelperAsync(file, path, currentDirectory);
        }

        private async Task<string> UploadFileHelperAsync(IFormFile file, string pathToCopyTo, string directory)
        {
            var filePath = $"{pathToCopyTo}/{Guid.NewGuid()}-{file.FileName}";
            using var stream = new MemoryStream();
            await using var fileStream = new FileStream($"{directory}/{filePath}", FileMode.Create);
            await file.CopyToAsync(fileStream);
            return $"{_hostDomain}/{filePath}";
        }
    }
}