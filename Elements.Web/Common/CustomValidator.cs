using Microsoft.AspNetCore.Http;

namespace Elements.Web.Common
{
    public static class CustomValidator
    {
        //public static bool ValidateObject(object obj)
        //{
        //    if (obj != null)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        public static bool IsFormFileLenghtSmallerOrEqualTo(IFormFile formFile, long lenght)
        {
            if (formFile != null)
            {
                if (formFile.Length <= lenght)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsFormFileLenghtBiggerThan(IFormFile formFile, long lenght)
        {
            if (formFile != null)
            {
                if (formFile.Length > lenght)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsFormFileInFormat(IFormFile formFile, params string[] formats)
        {
            if (formFile != null && formats.Length > 0)
            {
                foreach (var format in formats)
                {
                    if (formFile.ContentType == format)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
