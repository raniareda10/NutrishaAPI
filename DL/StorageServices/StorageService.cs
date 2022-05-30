using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DL.EntitiesV1.Media;
using DL.EntitiesV1.Reactions;
using DL.Enums;
using DL.Extensions;
using DL.HelperInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace DL.StorageServices
{
    public class StorageService : IStorageService
    {
        private readonly string _hostDomain;
        private string _currentDirectory;
        public StorageService(IConfiguration configuration)
        {
            _hostDomain = configuration["Domain"];
        }
        public async Task<IList<MediaFile>> UploadAsync(IMedia model,
            string entityId,
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
            _currentDirectory = Directory.GetCurrentDirectory() + "/wwwroot";
            
            var path = $"/{entityType}-{entityId}";
            Directory.CreateDirectory($"{_currentDirectory}{path}");
            foreach (var fileModel in model.Files)
            {
                mediaFiles.Add(new MediaFile()
                {
                    Id = Guid.NewGuid(),
                    Url = await UploadFileAsync(fileModel.File, path),
                    Flags = fileModel.Flags,
                    MediaType = MediaExtensions.GetFileType(fileModel.File),
                    Thumbnail = null
                });
            }

            return mediaFiles;
        }

        private async Task<string> UploadFileAsync(IFormFile file, string pathToCopyTo)
        {
            var extension = Path.GetExtension(file.FileName);
            var filePath = $"{pathToCopyTo}/{Guid.NewGuid()}{extension}";
            using var stream = new MemoryStream();
            await using var fileStream = new FileStream($"{_currentDirectory}{filePath}", FileMode.Create);
            await file.CopyToAsync(fileStream);
            return $"{_hostDomain}{filePath}";
        }
    }
}