using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using System.Globalization;

namespace EasyScholarRanking.Data
{
    public static class AuthorFileService
    {
        //private static string contentUrl = ConfigurationManager.AppSettings["HostedUrl"];

        private static string FileName
        {
            get { return Path.GetFullPath("Data/spanish-authors.txt"); }
        }


        public static bool SearchAuthor(string authorName)
        {
            authorName = authorName.ToLower();
            authorName = RemoveDiacritics(authorName);

            foreach (string line in File.ReadLines(FileName))
            {
                string currentLine = line.ToLower();
                currentLine = RemoveDiacritics(currentLine);

                if (currentLine.Equals(authorName))
                {
                    return true;
                }
            }

            return false;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
