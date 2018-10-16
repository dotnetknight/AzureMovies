using AzureMovies.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AzureMovies.Controllers
{
    public class SearchController : Controller
    {
        public static string MovieTitle;
        public static string ReleaseDate;
        public static string PosterPath;
        public static string Overview;
        public static string TagLine;
        public static string Lines;
        public static string Genres;
        public static string Runtime;
        public static string OriginalLanguage;
        public static string Director;
        public static string DirectorName = "";
        public static string ScreenPlay = "";
        public static string Composer = "";
        public static string TrailerVideoUrl = "";
        public static string Writer = "";
        public static int CastCount = 0;
        public static int CrewCount = 0;
        public static int ScreenPlaysCount = 0;
        public static int WritersCount = 0;
        public static int DirectorJobCount = 0;
        public static int ScreenPlayJobCount = 0;
        public static int ComposerJobCount = 0;
        public static int WriterJobCount = 0;
        public static int Revenue;
        public static int Budget;

        public static List<GetCredit.Cast> ActorName;
        public static List<GetCredit.Crew> FullCrewData;
        public static string ActorPhoto;
        public static string CharacterName;
        public static List<GetSearchResults> SearchResults = new List<GetSearchResults>();
        public static List<GetSearchResults> SearchResults1 = new List<GetSearchResults>();
        public static string Data = "";
        public static List<string> Res = new List<string>();

        [HttpGet]
        public ActionResult Search() { return View(); }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Search(Search query)
        {
            if (ModelState.IsValid)
            {
                string url = @"https://api.themoviedb.org/3/search/movie?api_key=01eb700d027839712c6317e7a1a9bc32&language=en-US&query=" + query.Keyword + "&page=1&include_adult=false";
                string JSonString = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) { using (Stream stream = response.GetResponseStream()) { using (StreamReader reader = new StreamReader(stream)) { JSonString = reader.ReadToEnd(); reader.Close(); stream.Close(); } } }

                var list = JsonConvert.DeserializeObject<SearchResultRootObject>(JSonString);
                SearchResults = list.results.ToList();
                return View("SearchResult");
            }
            else { ModelState.AddModelError("SearchError", "Search query is required"); return View(); }
        }

        [HttpPost]
        public ActionResult GetData(string btnId)
        {
            DirectorName = "";
            ScreenPlay = "";
            Composer = "";
            Writer = "";
            string MovieId = Request["btnId"].ToString();
            if (MovieId != "" || MovieId != null)
            {
                string url = "https://api.themoviedb.org/3/movie/" + MovieId + "?api_key=01eb700d027839712c6317e7a1a9bc32&language=en-US";
                string JSonString = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) { using (Stream stream = response.GetResponseStream()) { using (StreamReader reader = new StreamReader(stream)) { JSonString = reader.ReadToEnd(); reader.Close(); stream.Close(); } } }

                var MovieData = JsonConvert.DeserializeObject<AzureMovies.GetMovieData.GetMovieDataRootObject>(JSonString);

                string CastUrl = "https://api.themoviedb.org/3/movie/" + MovieId + "/credits?api_key=01eb700d027839712c6317e7a1a9bc32";
                string CastJSonString = "";
                HttpWebRequest CastRequest = (HttpWebRequest)WebRequest.Create(CastUrl);
                CastRequest.Method = "GET";

                using (HttpWebResponse CastResponse = (HttpWebResponse)CastRequest.GetResponse()) { using (Stream CastStream = CastResponse.GetResponseStream()) { using (StreamReader CastDataReader = new StreamReader(CastStream)) { CastJSonString = CastDataReader.ReadToEnd(); } } }

                var CastData = JsonConvert.DeserializeObject<AzureMovies.GetCredit.CreditRootObject>(CastJSonString);

                string TrailerUrl = "https://api.themoviedb.org/3/movie/" + MovieId + "/videos?api_key=01eb700d027839712c6317e7a1a9bc32&language=en-US";
                string TrailerJSon = "";

                HttpWebRequest VideoRequest = (HttpWebRequest)WebRequest.Create(TrailerUrl);
                VideoRequest.Method = "GET";

                using (HttpWebResponse VideoResponse = (HttpWebResponse)VideoRequest.GetResponse()) { using (Stream stream = VideoResponse.GetResponseStream()) { using (StreamReader reader = new StreamReader(stream)) { TrailerJSon = reader.ReadToEnd(); reader.Close(); stream.Close(); } } }
                //var TrailerData = JsonConvert.DeserializeObject<
                var TrailerData = JsonConvert.DeserializeObject<TrailerRootObject>(TrailerJSon);
                List<GetTrailer> TrailerDataList = new List<GetTrailer>();
                TrailerDataList = TrailerData.results.ToList();

                int RandomTrailerIndex = 0;
                if (TrailerDataList.Count == 1) { RandomTrailerIndex = 0; }
                if (TrailerDataList.Count > 1) { Random RandomTrailer = new Random(); RandomTrailerIndex = RandomTrailer.Next(0, TrailerDataList.Count); }
                TrailerVideoUrl = "http://www.youtube.com/embed/" + TrailerDataList[RandomTrailerIndex].key;

                string j = "";
                for (int i = 0; i < MovieData.genres.Count; i++) { j += MovieData.genres[i].name.ToString() + ", "; }
                string myString = j;
                myString = myString.Remove(myString.Length - 2);
                Genres = myString;

                DirectorJobCount = 0;
                ScreenPlayJobCount = 0;
                ComposerJobCount = 0;
                WriterJobCount = 0;
                for (int i = 0; i < CastData.crew.Count; i++)
                {
                    if (CastData.crew[i].job == "Director") { DirectorJobCount++; DirectorName += CastData.crew[i].name + ", "; }
                    if (CastData.crew[i].job == "Screenplay") { ScreenPlayJobCount++; ScreenPlay += CastData.crew[i].name + ", "; }
                    if (CastData.crew[i].job == "Original Music Composer") { ComposerJobCount++; Composer += CastData.crew[i].name + ", "; }
                    if (CastData.crew[i].job == "Writer") { WriterJobCount++; Writer += CastData.crew[i].name + ", "; }
                }
                ScreenPlaysCount = ScreenPlayJobCount;
                WritersCount = WriterJobCount;

                if (DirectorJobCount >= 1) { DirectorName = DirectorName.Remove(DirectorName.Length - 2); }
                if (ScreenPlayJobCount >= 1) { ScreenPlay = ScreenPlay.Remove(ScreenPlay.Length - 2); }
                if (ComposerJobCount >= 1) { Composer = Composer.Remove(Composer.Length - 2); }
                if (WriterJobCount >= 1) { Writer = Writer.Remove(Writer.Length - 2); }

                int Hours;
                int Minutes;
                int SumMins;
                int MinutesToConvert = MovieData.runtime;
                Hours = MinutesToConvert / 60;
                Minutes = MovieData.runtime;
                SumMins = Minutes - (Hours * 60);
                Runtime = Hours.ToString() + "h " + SumMins.ToString() + "m";

                PosterPath = "http://image.tmdb.org/t/p/original" + MovieData.poster_path;
                Overview = MovieData.overview;
                MovieTitle = MovieData.title;
                ReleaseDate = MovieData.release_date;
                Budget = MovieData.budget;
                Revenue = MovieData.revenue;
                TagLine = MovieData.tagline;
                OriginalLanguage = MovieData.original_language;

                CastCount = CastData.cast.Count;
                CrewCount = CastData.crew.Count;
                ActorName = CastData.cast.ToList();
                FullCrewData = CastData.crew.ToList();
                return View();
            }
            else { return View(); }
        }
        public ActionResult FullCast() { return View(); }

        public ActionResult FullCrew() { return View(); }

        [HttpPost]
        public JsonResult PostMethod(string name)
        {
            if (ModelState.IsValid)
            {
                if (name != null || name != "" || name != string.Empty)
                {
                    try
                    {
                        string url = @"https://api.themoviedb.org/3/search/movie?api_key=01eb700d027839712c6317e7a1a9bc32&language=en-US&query=" + name + "&page=1&include_adult=false";
                        string JSonString = "";

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.Method = "GET";

                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) { using (Stream stream = response.GetResponseStream()) { using (StreamReader reader = new StreamReader(stream)) { JSonString = reader.ReadToEnd(); reader.Close(); stream.Close(); } } }

                        var list = JsonConvert.DeserializeObject<SearchResultRootObject>(JSonString);
                        SearchResults1.Clear();
                        SearchResults1 = list.results.ToList();

                        /*
                        string Titles = "";
                        for (int i = 0; i < 2; i++)
                        {
                            if (SearchResults.Count != 0) { Titles = SearchResults; }
                        }
                         */

                        PersonModel person = new PersonModel { MovieTitles = SearchResults1, ResultCount = SearchResults.Count };

                        return Json(person);
                    }
                    catch { }
                }
                else { return Json("", JsonRequestBehavior.AllowGet); }
            }
            else { ModelState.AddModelError("Val", "Value must be present"); return Json("", JsonRequestBehavior.AllowGet); }
            PersonModel person1 = new PersonModel { ResultCount = 1009 };
            return Json(person1);
        }
    }
}