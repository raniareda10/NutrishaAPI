using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DL.DtosV1.Common;
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

        public async Task<IList<MediaFile>> PrepareMediaAsync(IMedia model,
            // string entityId,
            EntityType entityType,
            IEnumerable<MediaFile> oldMedia = null,
            HashSet<Guid> removedMedia = null)
        {
            oldMedia ??= new List<MediaFile>();
            if (removedMedia is { Count: > 0 })
            {
                oldMedia = oldMedia?
                    .Where(m => !removedMedia.Contains(m.Id))
                    .ToList() ?? new List<MediaFile>();
            }
            
            var filesCount = model.Files?.Count ?? 0;
            var externalMediaCount = model.ExternalMedia?.Count ?? 0;
            var count = filesCount + externalMediaCount;

            if (count == 0) return oldMedia.ToList();
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

            return mediaFiles.Concat(oldMedia).ToList();
        }

        public async Task<IList<MediaFile>> PrepareMediaAsync(IList<MediaFileDto> mediaFiles, EntityType entityType)
        {
            if (mediaFiles == null || mediaFiles.Count == 0) return null;


            var files = new List<MediaFile>(mediaFiles.Count);

            foreach (var mediaFile in mediaFiles)
            {
                files.Add(await PrepareMediaAsync(mediaFile, entityType));
            }

            return files;
        }

        public async Task<MediaFile> PrepareMediaAsync(MediaFileDto mediaFile, EntityType entityType)
        {
            if (mediaFile.File == null)
                return new MediaFile()
                {
                    Id = Guid.NewGuid(),
                    Url = mediaFile.Url,
                    Flags = mediaFile.Flags.Select(m => m.ToString()).ToHashSet(),
                    MediaType = MediaExtensions.ExtractExternalUrlType(mediaFile.Url),
                    Thumbnail = null
                };


            var path = $"{entityType}-{Guid.NewGuid()}";
            return new MediaFile()
            {
                Id = Guid.NewGuid(),
                Flags = mediaFile.Flags.Select(m => m.ToString()).ToHashSet(),
                MediaType = MediaExtensions.GetFileType(mediaFile.File),
                Url = await UploadFileAsync(mediaFile.File, path)
            };
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