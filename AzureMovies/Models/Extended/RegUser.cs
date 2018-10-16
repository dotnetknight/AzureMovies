using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AzureMovies.Models
{
    [MetadataType(typeof(UserMetaData))]
    public partial class RegUser { public string ConfirmPassword { get; set; } }

    public class UserMetaData
    {
        [Display(Name = "First name:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required")]
        public string First_Name { get; set; }

        [Display(Name = "Last name:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name required")]
        public string Last_Name { get; set; }

        [Display(Name = "Email:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Password:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be 6 character long")]
        public string Password_Hash { get; set; }

        [Display(Name = "Confirm:")]
        [DataType(DataType.Password)]
        [Compare("Password_Hash", ErrorMessage = "Password doesn't match")]
        public string ConfirmPassword { get; set; }
    }
}