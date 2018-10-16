using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AzureMovies.Models
{
    public class Search
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a keyword")]
        public string Keyword { get; set; }
    }
}