using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

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
            foreach (string line in File.ReadLines(FileName))
            {
                if (line.ToLower().Equals(authorName))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
