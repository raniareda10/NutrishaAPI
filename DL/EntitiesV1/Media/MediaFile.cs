using System;

namespace DL.EntitiesV1.Media
{
    public class MediaFile
    {
        public Guid Id { get; set; }
        public MediaType MediaType { get; set; }
        public string Url { get; set; }
        public string Thumbnail { get; set; }
        public string[] Flags { get; set; }
    }
}