using System;
using System.Collections.Generic;

namespace DL.EntitiesV1.Media
{
    public class MediaFile
    {
        public Guid Id { get; set; }
        public MediaType MediaType { get; set; }
        public string Url { get; set; }
        public string Thumbnail { get; set; }
        public HashSet<string> Flags { get; set; }
    }
}