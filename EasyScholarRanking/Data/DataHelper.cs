using ChartJs.Blazor.Util;
using EasyScholarRanking.Models;
using System;
using System.Collections.Generic;

namespace EasyScholarRanking.Data
{
    public class DataHelper
    {
        #region Venue List Operations
        // Given a venue name return the key or substitute spaces by underscores
        public static string ParseVenue(string venue, List<Venue> venueList)
        {
            foreach (Venue v in venueList)
            {
                if (venue.Equals(v.Name))
                {
                    return v.Key;
                }
            }

            return venue.Replace(' ', '_');
        }
        #endregion

        #region Author List Operations
        // Given an author list, calculate the number of publications in it
        public static int GetTotalPublications(List<AuthorClass> authorList)
        {
            int total = 0;

            foreach (AuthorClass author in authorList)
            {
                total += author.Score;
            }

            return total;
        }

        // Given an author list and a score, calculate the amount of authors with that same score
        public static int CountAuthorByScore(int score, List<AuthorClass> authorList)
        {
            int number = 0;

            foreach (AuthorClass author in authorList)
            {
                if (author.Score == score)
                {
                    number++;
                }
                else if (author.Score < score)
                {
                    break;
                }
            }

            return number;
        }

        // Given an author list and an author name, get the author object
        public static AuthorClass GetAuthorByName(string name, List<AuthorClass> authorList)
        {
            name = name.ToLower();

            for(int i = 0; i < authorList.Count; ++i)
            {
                string currentAuthorName = authorList[i].Text.ToLower();
                if (currentAuthorName.Equals(name))
                {
                    authorList[i].Position = GetAuthorPosition(i, authorList);
                    return authorList[i];
                }
            }

            return null;
        }

        // Get real position of an author inside a ranking
        private static int GetAuthorPosition(int pos, List<AuthorClass> authorList)
        {
            int currentPosScore = authorList[0].Score;
            int currentPos = 1;

            for (int i = 0; i <= pos; i++)
            {
                if(currentPosScore > authorList[i].Score)
                {
                    currentPos = (i + 1);
                    currentPosScore = authorList[i].Score;
                }
            }

            return currentPos;
        }

        // Calculate the different amount of scores for a given list of authors
        private static List<int> GetDifferentScoreValues(List<AuthorClass> authorList)
        {
            int value = 0;
            List<int> valueList = new List<int>();

            foreach(AuthorClass author in authorList)
            {
                if(value != author.Score)
                {
                    valueList.Add(author.Score);
                    value = author.Score;
                }
            }

            return valueList;
        }

        public static int GetPercentile(int score, List<AuthorClass> authorList)
        {
            int total = authorList.Count;

            // Create dictionary to store score-amount map
            Dictionary<int, int> scoreAmountPairs = new Dictionary<int, int>();

            // Get different values
            List<int> valueList = GetDifferentScoreValues(authorList);

            // Get amount of authors by score value
            foreach(int value in valueList)
            {
                int amount = CountAuthorByScore(value, authorList);
                scoreAmountPairs.Add(value, amount);
            }

            // Obtain percentage of entries with less score
            float percentage = 0;

            foreach (KeyValuePair<int, int> entry in scoreAmountPairs)
            {
                if(entry.Key < score)
                {
                    float entryPercentage = entry.Value / (float)total;
                    percentage += entryPercentage;
                }
            }

            // Obtain percentage of entries with same score
            float currentScorePercentage = scoreAmountPairs[score] / (float)total;

            // Halve result to get average score
            currentScorePercentage /= 2;

            // Calculate final percentage as percentile
            float percentileFloat = currentScorePercentage + percentage;

            // Convert float to int
            int percentile = (int)(percentileFloat * 100);

            return percentile;
        }
        #endregion

        #region Location List Operations
        public static List<string> GetLocalSearchLocations()
        {
            return new List<string>() { "International", "Spain" };
        }
        #endregion

        #region Author Operations
        // Given the number of publications and authors, give the text it should show on screen
        public static string GetMeanText(int numPublications, int numAuthors)
        {

            if (numPublications == 0 && numAuthors == 0)
            {
                return " - No publications found!";
            }

            string meanText = "";

            try
            {

                double mean = GetMean(numPublications, numAuthors);

                if (mean != 0)
                {
                    return " - " + mean + " publications by author";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return meanText;
        }

        // Given two numbers calculate the mean
        public static double GetMean(int a, int b)
        {
            double mean = (double)a / (double)b;
            return Math.Round(mean, 2);
        }

        // Given a percentile calculate the quartile
        public static string GetQuartile(int percentile) // 0-24 = 0, 25-49 = 1, 50-74 = 2, 75-99 = 3
        {
            int quartile = 4 - (percentile / 25);

            return "Q" + quartile;
        }

        #endregion

        #region Chart Operations
        // Given a number create a color array of that size
        public static string[] GetBackgroundColors(int size)
        {
            string[] colorArray = new string[size];

            Random random = new Random(DateTime.Now.Millisecond);
            int a, b, c;

            for (int i = 0; i < size; i++)
            {
                a = random.Next(256);
                b = random.Next(256);
                c = random.Next(256);
                colorArray[i] = ColorUtil.ColorHexString((byte)a, (byte)b, (byte)c);
            }

            return colorArray;
        }
        #endregion

        #region Info Query Operations

        // Create the string to show once a ranking search query has been done
        public static string GetRankingInfoQuery(string venue, string location, int minYear, int maxYear, int entries, int careerLenght)
        {
            string timePeriod = minYear.ToString();

            if (minYear != maxYear)
            {
                timePeriod += " - " + maxYear.ToString();
            }

            string infoQuery = string.Format("Venue: {0}, Location: {1}, Time period: {2}, Entries: {3}, Min. Active Years in Query: {4}.", venue, location, timePeriod, entries.ToString(), careerLenght.ToString());

            return infoQuery;
        }

        // Create the string to show once an author search query has been done
        public static string GetAuthorInfoQuery(string venue, string location, int minYear, int maxYear, string author)
        {
            string timePeriod = minYear.ToString();

            if (minYear != maxYear)
            {
                timePeriod += " - " + maxYear.ToString();
            }

            string infoQuery = string.Format("Author: {0}, Venue: {1}, Location: {2}, Time period: {3}.", author, venue, location, timePeriod);

            return infoQuery;
        }
        #endregion
    }
}
