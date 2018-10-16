using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace AzureMovies
{
    public class GetPopular
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

    public class PopularRootObject
    {
        public int page { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
        public List<GetPopular> results { get; set; }
    }

    public class GetPopularData
    {
        public static List<GetPopular> GetData()
        {
            string JSonString = "";
            string url = @"https://api.themoviedb.org/3/movie/popular?api_key=01eb700d027839712c6317e7a1a9bc32&language=en-US&page=1";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    { JSonString = reader.ReadToEnd(); reader.Close(); stream.Close(); }
                }
            }

            var list = JsonConvert.DeserializeObject<PopularRootObject>(JSonString);
            List<GetPopular> Popular = new List<GetPopular>();
            Popular = list.results.ToList();

            return Popular;
        }
    }
}