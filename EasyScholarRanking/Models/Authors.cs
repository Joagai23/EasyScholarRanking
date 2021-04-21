using System.Collections.Generic;

namespace EasyScholarRanking.Models
{
    public class Authors
    {
        public Authors(List<AuthorClass> AuthorList) => Author = AuthorList;
        public List<AuthorClass> Author { get; set; }
    }

    public class AuthorClass
    {
        public string Text { get; set; }
        public int Score { get; set; }
        public string Pid { get; set; }
        public int FirstYear { get; set; }
        public int LastYear { get; set; }
        public int Position { get; set; }
        public string Venue { get; set; }
        public List<string> VenueList { get; set; }

        public int GetYearDifference()
        {
            if (FirstYear != 0 && LastYear != 0)
            {
                return (LastYear - FirstYear);
            }

            return 0;
        }

        public void SetPublicationYear(int year)
        {
            if (FirstYear == 0)
            {
                FirstYear = LastYear = year;
            }
            else
            {   
                if(year < FirstYear)
                {
                    FirstYear = year;
                }
                else if (year > LastYear)
                {
                    LastYear = year;
                }
            }
            
        }

        public void IncreaseScore()
        {
            Score++;
        }

        public void AddVenue(string venueName)
        {
            if(VenueList == null)
            {
                VenueList = new List<string>();
            }

            if (!VenueList.Contains(venueName))
            {
                VenueList.Add(venueName);
            }
        }
    }
}
