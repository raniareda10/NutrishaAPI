using System.Collections.Generic;
using DL.DtosV1.Storage;

namespace DL.HelperInterfaces
{
    public interface IMedia
    {
        public IList<FormFileDto> Files { get; set; }
        public IList<ExternalMedia> ExternalMedia { get; set; }
    }
}