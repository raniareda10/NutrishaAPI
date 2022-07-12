using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DL.DtosV1.Common;
using DL.DtosV1.Storage;
using DL.EntitiesV1.Media;
using DL.Enums;
using DL.HelperInterfaces;
using Microsoft.AspNetCore.Http;

namespace DL.StorageServices
{
    public interface IStorageService
    {
        Task<IList<MediaFile>> PrepareMediaAsync(IMedia model,
            EntityType entityType,
            IEnumerable<MediaFile> oldMedia = null,
            HashSet<Guid> removedMedia = null);

        Task<IList<MediaFile>> PrepareMediaAsync(IList<MediaFileDto> mediaFiles, EntityType entityType);
        Task<MediaFile> PrepareMediaAsync(MediaFileDto mediaFile, EntityType entityType);

        Task<string> UploadFileAsync(IFormFile file, string path);
    }
}