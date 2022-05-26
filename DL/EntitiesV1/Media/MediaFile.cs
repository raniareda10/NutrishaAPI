namespace DL.EntitiesV1.Media
{
    public class MediaFile
    {
        public MediaType MediaType { get; set; }
        public string Url { get; set; }
        public string[] Flags { get; set; }
    }
}