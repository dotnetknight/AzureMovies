using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AzureMovies.Models
{
    public class GetSearchResults
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

    public class SearchResultRootObject
    {
        public int page { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
        public List<GetSearchResults> results { get; set; }
    }

    public class PersonModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter text")]
        [Display(Name = "Text:")]
        public string Name { get; set; }

        public List<GetSearchResults> MovieTitles { get; set; }
        public int ResultCount { get; set; }
    }
}