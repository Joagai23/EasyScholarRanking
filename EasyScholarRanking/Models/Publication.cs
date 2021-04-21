using System.Collections.Generic;

namespace EasyScholarRanking.Models
{
    public class Publication
    {
        public string Type { get; set; }
        public string Year { get; set; }
        public string Venue { get; set; }
        public string Title { get; set; }
        public List<string> AuthorList { get; set; }
    }
}
