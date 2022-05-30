using System.Collections.Generic;
using System.Threading.Tasks;
using DL.DtosV1.Storage;
using DL.EntitiesV1.Media;
using DL.Enums;
using DL.HelperInterfaces;
using Microsoft.AspNetCore.Http;

namespace DL.StorageServices
{
    public interface IStorageService
    {
        Task<IList<MediaFile>> UploadAsync(IMedia model, 
            string entityId,
            EntityType entityType);
    }
}