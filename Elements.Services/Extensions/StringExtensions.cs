namespace Elements.Services.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxLength)
        {
            int minLenght = 0;
            string result = string.Empty;
            string subfix = "...";

            if (value != null)
            {
                if (value.Length > maxLength)
                {
                    minLenght = value.LastIndexOf(' ', maxLength);

                    if (minLenght <= 0)
                    {
                        minLenght = maxLength;
                    }
                }
                else
                {
                    minLenght = value.Length;
                    subfix = string.Empty;
                }

                result = value.Substring(0, minLenght).TrimEnd() + subfix;
            }

            return result;
        }
    }
}
