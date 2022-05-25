using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Helpers
{
    public interface IFileHelper
    {

    }
   public class FileHelper:IFileHelper
    {
      public static string FileUpload(IFormFile File,IHostingEnvironment hostingEnvironment,string UploadFolder)
        {
          
            string UniqeFileName = null;
            if (File!=null)
            {
               var path= Path.Combine("wwwroot", UploadFolder);
                UniqeFileName = Guid.NewGuid().ToString() + "-" + File.FileName;
                string FilePath = Path.Combine(path, UniqeFileName);
                File.CopyTo(new FileStream(FilePath, FileMode.Create));
               
                return UniqeFileName;
            }
            return UniqeFileName;
        }
     
      public static string DeleteFile(string FileName,string FolderFile,IHostingEnvironment hostingEnvironment)
        {
            try
            {
                string webRootPath = hostingEnvironment.WebRootPath;
                var path = Path.Combine(webRootPath, FolderFile);
                // Check if file exists with its full path    
                var fulpath = Path.Combine(path, FileName);
                if (File.Exists(fulpath))
                {
                    // If file found, delete it    
                    File.Delete(Path.Combine(path, FileName));
                    return "File deleted.";
                }
                else return "File not found";
            }
            catch (IOException ioExp)
            {
               return ioExp.Message;
            }
        }



        public static Image GetReducedImage(int width, int height, Stream resourceImage)
        {
            try
            {
                var image = Image.FromStream(resourceImage);
                var thumb = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);

                return thumb;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static bool CreateFolder(IHostingEnvironment hostingEnvironment, string FolderName)
        {
            string wwwPath = hostingEnvironment.WebRootPath;
            string contentPath = hostingEnvironment.ContentRootPath;

            string path = Path.Combine(hostingEnvironment.WebRootPath, FolderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return true;
            }
            return false;


        }
    }
}
