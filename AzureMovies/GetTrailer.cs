using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureMovies
{
    public class GetTrailer
    {
        public string id { get; set; }
        public string iso_639_1 { get; set; }
        public string iso_3166_1 { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string site { get; set; }
        public int size { get; set; }
        public string type { get; set; }
    }

    public class TrailerRootObject
    {
        public int id { get; set; }
        public List<GetTrailer> results { get; set; }
    }
}