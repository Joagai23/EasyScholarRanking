using System.Text.Json;

namespace EasyScholarRanking.Models
{
    public class Venue
    {
        public string Name { get; set; }
        public string Key { get; set; }

        public override string ToString() => JsonSerializer.Serialize<Venue>(this);
    }
}
