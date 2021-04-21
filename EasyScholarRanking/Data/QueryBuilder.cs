using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyScholarRanking.Data
{
    public class QueryBuilder
    {
        private static readonly string publicationQuery = "https://dblp.org/search/publ/api?q=";
        private static readonly string authorQuery = "https://dblp.org/search/author/api?q=";
        private static readonly int maxNumberResults = 300;

        public static string BuildQueryVenueYear(string venue, string year)
        {
            string query = publicationQuery;

            // Append Get Request with options
            query = AppendQuery("venue", venue, query);
            query = AppendQuery("year", year, query);
            query = AppendRequest("h", maxNumberResults.ToString(), query);
            return AppendRequest("format", "json", query);
        }

        public static string BuildQueryAuthor(string author)
        {
            string query = authorQuery;

            author = author.Replace(" ", "_");
            query += author; 

            return AppendRequest("format", "json", query);
        }

        private static string AppendQuery(string term, string value, string query)
        {
            return query += string.Format("{0}:{1}:", term, value);
        }

        private static string AppendRequest(string type, string value, string query)
        {
            return query += string.Format("&{0}={1}", type, value);
        }
    }
}
