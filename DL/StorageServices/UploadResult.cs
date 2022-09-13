using System;

namespace DL.StorageServices
{
    public class UploadResult
    {
        public DateTime Created { get; set; }
        public string FileExtension { get; set; }
        public string Url { get; set; }
    }
}