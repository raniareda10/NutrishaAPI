using System;
using System.IO;
using System.Text.RegularExpressions;
using DL.EntitiesV1.Media;
using Microsoft.AspNetCore.Http;
using MimeKit;

namespace DL.Extensions
{
    public static class MediaExtensions
    { 
        public static MediaType ExtractExternalUrlType(string url)
        {
            var regexYoutube = new Regex(@"^(https?:\/\/)?((www\.|m\.)?youtube.com|youtu.be)\/(watch)?(\?v=)?(\S+)?");
            var regexFacebook1 = new Regex(@"^(https?:\/\/)?(?:www\.|web\.|m\.)?facebook\.com\/.+");
            var regexFacebook2 = new Regex(@"^(https?:\/\/)?(?:www\.|web\.|m\.|fb\.)?watch\/.+");
            // var regexInstagram = new Regex(@"^(((https?:\/\/)?(?:www\.)?instagram\.com\/.*\/([^\/?#&]+)).*)");
            // var regexTwitter = new Regex(@"^(https?:\/\/)?(?:www\.)?twitter\.com\/(?:#!\/)?(\w+)\/status(es)?\/(\d+)(\?.*)?(?:\/.*)?$");
            return 
                regexYoutube.IsMatch(url) ? MediaType.Youtube :
                regexFacebook1.IsMatch(url) | regexFacebook2.IsMatch(url) ? MediaType.Facebook :
                throw new ArgumentException("Only Facebook Or Youtube Urls");
        }
        
        
        public static MediaType GetFileType(IFormFile file)
        {
            var type = MimeTypes.GetMimeType(Path.GetExtension(file.FileName)).ToLower();
            
             return type.StartsWith("image") ? MediaType.Image :
                 type.StartsWith("video") ? MediaType.Video :
                 throw new ArgumentException("Only Images Or Videos");
        }
    }
}