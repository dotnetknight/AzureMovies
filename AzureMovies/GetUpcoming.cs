using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace AzureMovies
{
    public class GetUpcoming
    {
        public int vote_count { get; set; }
        public int id { get; set; }
        public bool video { get; set; }
        public double vote_average { get; set; }
        public string title { get; set; }
        public double popularity { get; set; }
        public string poster_path { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public List<int> genre_ids { get; set; }
        public string backdrop_path { get; set; }
        public bool adult { get; set; }
        public string overview { get; set; }
        public string release_date { get; set; }
    }

    public class UpcomingDates
    {
        public string maximum { get; set; }
        public string minimum { get; set; }
    }

    public class UpcomingRootObject
    {
        public List<GetUpcoming> results { get; set; }
        public int page { get; set; }
        public int total_results { get; set; }
        public UpcomingDates dates { get; set; }
        public int total_pages { get; set; }
    }

    public class GetUpcomingData
    {
        public static List<GetUpcoming> GetData()
        {
            string JSonString = "";
            string url = @"https://api.themoviedb.org/3/movie/upcoming?api_key=01eb700d027839712c6317e7a1a9bc32&language=en-US&page=1";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    { JSonString = reader.ReadToEnd(); reader.Close(); stream.Close(); }
                }
            }

            var list = JsonConvert.DeserializeObject<UpcomingRootObject>(JSonString);
            List<GetUpcoming> UpComing = new List<GetUpcoming>();
            UpComing = list.results.ToList();

            return UpComing;
        }
    }
}