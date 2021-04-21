using EasyScholarRanking.Models;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EasyScholarRanking.Data
{
    public static class JsonFileVenueService
    {
        private static string JsonFileName
        {
            get { return Path.GetFullPath("Data/venues.json"); }
        }

        public static IEnumerable<Venue> GetVenues()
        {
            using var jsonFileReader = File.OpenText(JsonFileName);
            return JsonSerializer.Deserialize<Venue[]>(jsonFileReader.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
    }
}
