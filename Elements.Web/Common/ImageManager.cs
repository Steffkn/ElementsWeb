using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Elements.Web.Common
{
    public class ImageManager
    {
        private static string imagePathFormat = "/images/{0}/{1}";
        private static string newImageNameFormat = "{0}-{1}";
        private static string rootDirectory = "wwwroot";

        public static string GetNewFileName(string fileName)
        {
            return string.Format(newImageNameFormat, Guid.NewGuid().ToString("N"), fileName);
        }

        public static string GetIconRelativePath(string customFolderName, string fileName)
        {
            return string.Format(imagePathFormat, customFolderName, fileName);
        }

        public static string GetFullFilePath(string customFolderName, string fileName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), rootDirectory, "images", customFolderName, fileName);
        }

        public static async Task UploadFileAsync(string fullFilePathName, IFormFile formFile)
        {
            using (var fileStream = new FileStream(fullFilePathName, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }
        }
    }
}
