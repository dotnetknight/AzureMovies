using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureMovies.Models;

namespace AzureMovies.Controllers
{
    public class HomeController : Controller
    {
        public static string Name = "";
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                using (MoviesEntities dc = new MoviesEntities())
                {
                    string email = HttpContext.User.Identity.Name;
                    var v = dc.RegUsers.Where(a => a.Email == email).FirstOrDefault();
                    if (v != null) { Name = v.First_Name; }
                }
            }
            return View();
        }
    }
}