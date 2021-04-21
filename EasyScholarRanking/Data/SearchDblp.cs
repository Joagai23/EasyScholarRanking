using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using EasyScholarRanking.Models;
using System;

namespace EasyScholarRanking.Data
{
    public static class SearchDblp
    {
        // Calculate and filter a ranking based on user's queries
        public static List<AuthorClass> GetRankingSearch(string venue, int minYear, int maxYear, int numEntries, int duration, string location, string name, string locationSearch)
        {
            Dictionary<string, AuthorClass> authorMap = GetDblpRanking(venue, minYear, maxYear, name, location, locationSearch);

            if (maxYear - minYear < duration)
            {
                duration = maxYear - minYear;
            }

            return FilterMap(authorMap, numEntries, duration, location, locationSearch);
        }

        // Calculate and filter a ranking called by AuthosSearch with no max. number of entries and min.duration of career
        public static List<AuthorClass> GetIndividualVenueRanking(string venue, int minYear, int maxYear, string location, string name, string locationSearch)
        {
            return GetRankingSearch(venue, minYear, maxYear, int.MaxValue, 0, location, name, locationSearch);
        }

        // Get DBLP ranking for a determined number of years
        private static Dictionary<string, AuthorClass> GetDblpRanking(string venue, int minYear, int maxYear, string author, string location, string locationSearch)
        {
            if (ValidateInput(minYear, maxYear))
            {
                List<Authors> authorList = new List<Authors>();
                List<Authors> copyList = new List<Authors>();

                string bestVenue = "";
                int bestScore = 0;
                int bestPosition = int.MaxValue;

                if (venue.Equals("GSR"))
                {
                    List<Venue> venueList =  JsonFileVenueService.GetVenues().ToList();

                    foreach(Venue v in venueList)
                    {
                        // Clear copy list for every new venue
                        copyList.Clear();

                        for (int i = minYear; i <= maxYear; i++)
                        {
                            // Copy list now has all the info about a venue
                            copyList.AddRange(AddListByYear(v.Key, i));
                        }

                        //Add values of venue to author list
                        authorList.AddRange(copyList);

                        // If we require the best venue for the author
                        if(author != null)
                        {
                            List<AuthorClass> orderedList = FilterMap(OrderRanking(copyList), int.MaxValue, 0, location, locationSearch);
                            AuthorClass authorClass = DataHelper.GetAuthorByName(author, orderedList);

                            if(authorClass != null)
                            {
                                if(authorClass.Position <= bestPosition && authorClass.Score > bestScore)
                                {
                                    bestScore = authorClass.Score;
                                    bestPosition = authorClass.Position;
                                    bestVenue = v.Name;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = minYear; i <= maxYear; i++)
                    {
                        authorList.AddRange(AddListByYear(venue, i));
                    }
                }

                // If we require the best venue for the author
                if (author != null && bestScore != 0)
                {
                    AuthorClass bestAuthorClass = new AuthorClass() { Text = "BestVenue", Score = bestScore, FirstYear = minYear, LastYear = maxYear, Venue = bestVenue };
                    List<AuthorClass> authorClassList = new List<AuthorClass>() { bestAuthorClass };
                    Authors bestAuthors = new Authors(authorClassList);
                    authorList.Add(bestAuthors);
                }

                return OrderRanking(authorList);
            }

            return null;
        }

        // Return a list of the authors extracted from a query by year
        private static List<Authors> AddListByYear(string venue, int year)
        {
            // Create new list
            List<Authors> authorList = new List<Authors>();

            try
            {
                // Parse year to string
                string yearToString = year.ToString();

                // Build Query
                string query = QueryBuilder.BuildQueryVenueYear(venue, yearToString);

                // Send request
                string response = SendRequest(query);

                // Parse response into JSON
                JObject search = JObject.Parse(response);

                // Get JSON result objects into a list
                List<JToken> results = search["result"]["hits"]["hit"].Children().ToList();

                // Serialize JSON results into .NET objects
                foreach (JToken result in results)
                {
                    try
                    {
                        string authorString = result["info"]["authors"].ToString();
                        int numberAuthors = Regex.Matches(authorString, "text").Count;

                        if (numberAuthors == 1)
                        {
                            string authorClassString = result["info"]["authors"]["author"].ToString();
                            AuthorClass author = JsonConvert.DeserializeObject<AuthorClass>(authorClassString);

                            // Set año de publicacion para un autor
                            author.SetPublicationYear(year);

                            // Set venue del autor
                            author.Venue = venue;

                            List<AuthorClass> tempAuthorList = new List<AuthorClass>() { author };

                            authorList.Add(new Authors(tempAuthorList));
                        }
                        else
                        {
                            Authors authors = JsonConvert.DeserializeObject<Authors>(authorString);

                            // Set año de la publicacion y venue para todos los autores de la publicacion
                            foreach(AuthorClass au in authors.Author)
                            {
                                au.SetPublicationYear(year);
                                au.Venue = venue;
                            }

                            authorList.Add(authors);
                        }
                    }catch(Exception)
                    {
                        continue;
                    }                    
                }

                return authorList;
            }
            catch (Exception)
            {
                return authorList;
            }
        }

        // Order List of Authors based on the times they appear and return it truncated
        private static Dictionary<string, AuthorClass> OrderRanking(List<Authors> authorList)
        {
            try
            {
                // Use dictionary to store information <AuthorName, AuthorClass>
                var map = new Dictionary<string, AuthorClass>();

                // Give a score to the author based on the times they appear
                for (int i = 0; i < authorList.Count; ++i)
                {
                    Authors authors = authorList[i];
                    for (int j = 0; j < authors.Author.Count; ++j)
                    {
                        string authorName = authors.Author[j].Text;

                        if (map.ContainsKey(authorName))  // Author already inside the dictionary
                        {
                            AuthorClass author = map.GetValueOrDefault(authorName);
                            author.IncreaseScore();
                            author.SetPublicationYear(authors.Author[j].LastYear);
                            author.AddVenue(authors.Author[j].Venue);
                        }
                        else // New Author
                        {
                            authors.Author[j].Score = 1;
                            authors.Author[j].AddVenue(authors.Author[j].Venue);
                            map.Add(authorName, authors.Author[j]);
                        }
                    }
                }

                return map;
            }
            catch (Exception)
            {
                return null;
            }

        }

        // Get filtered list by creating an ordered map
        public static List<AuthorClass> FilterMap(Dictionary<string, AuthorClass> map, int numberEntries, int duration, string location, string locationSearch)
        {
            var sortedDict = from entry in map orderby entry.Value.Score descending select entry;
            int currentEntry = 0;
            List<AuthorClass> ranking = new List<AuthorClass>();

            foreach (KeyValuePair<string, AuthorClass> entry in sortedDict)
            {
                if (currentEntry < numberEntries)
                {
                    // Filter by duration of career and location
                    if ((entry.Value.GetYearDifference() + 1) >= duration && CheckLocation(location, entry.Key, locationSearch))
                    {
                        ranking.Add(new AuthorClass() { Text = entry.Key, Score = entry.Value.Score, FirstYear = entry.Value.FirstYear, LastYear = entry.Value.LastYear, VenueList = entry.Value.VenueList});
                        currentEntry++;
                    }
                }
                else
                {
                    break;
                }
            }

            return ranking;
        }

        // Check Location of author
        private static bool CheckLocation(string location, string authorName, string locationSearch)
        {
            if (location.Equals("International") || authorName.Equals("BestAuthor"))
            {
                return true;
            }

            // If location search is local serch using the local database
            if (locationSearch.Equals("local"))
            {
                return SearchLocalAuthorDatabase(location, authorName);
            }

            try
            {
                // Build Query
                string query = QueryBuilder.BuildQueryAuthor(authorName);

                // Send request
                string response = SendRequest(query);

                // Parse response into JSON
                JObject search = JObject.Parse(response);

                // Get JSON result as list
                List<JToken> results = search["result"]["hits"]["hit"].Children().ToList();

                // Serialize JSON results and obtain affiliation
                foreach (JToken result in results)
                {
                    try
                    {
                        // Obtain relevant information
                        string authorString = result["info"]["notes"]["note"].ToString();
                        int numberAffiliations = Regex.Matches(authorString, "text").Count;

                        string affiliation = "";
                        string dblpName = result["info"]["author"].ToString();

                        if (numberAffiliations == 1)
                        {
                            // Get affiliation and name from JToken
                            affiliation = result["info"]["notes"]["note"]["text"].ToString();

                            // Only return true if affiliation contains all the location and the name is exactly the same as the one we are searching for
                            if (authorName.Equals(dblpName) && Regex.IsMatch(affiliation, $"\\b{location}\\b", RegexOptions.IgnoreCase))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            // Obtener lista de objetos dentro de las notas de la busqueda
                            var affiliationList = result["info"]["notes"]["note"].ToList();
                            foreach (var authorAffiliation in affiliationList)
                            {
                                // Convertir objeto a Affiliation para poder leer la propiedad del nombre
                                Affiliation currentAffiliation = JsonConvert.DeserializeObject<Affiliation>(authorAffiliation.ToString());
                                affiliation = currentAffiliation.Text;

                                // Only return true if affiliation contains all the location and the name is exactly the same as the one we are searching for
                                if (authorName.Equals(dblpName) && Regex.IsMatch(affiliation, $"\\b{location}\\b", RegexOptions.IgnoreCase))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    catch(Exception)
                    {
                        continue;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Send Request to server given user input
        private static string SendRequest(string url)
        {
            string content = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                content = reader.ReadToEnd();
            }

            return content;
        }

        // Check year input are valid
        private static bool ValidateInput(int minYear, int maxYear)
        {
            if (minYear <= maxYear)
            {
                return true;
            }

            return false;
        }

        // Search using local database
        private static bool SearchLocalAuthorDatabase(string location, string authorName)
        {
            // Search author name in local file
            return AuthorFileService.SearchAuthor(authorName);
        }
    }
}
