using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AzureMovies.Models;
using System.IO;

namespace AzureMovies.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Registration() { if (User.Identity.IsAuthenticated) { return RedirectToAction("Index", "Home"); } else { return View(); } }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Registration(RegUser user)
        {
            if (ModelState.IsValid)
            {
                var Email = IsEmailExists(user.Email);
                if (Email) { ModelState.AddModelError("EmailExists", "Email is already exists"); return View(user); }

                user.Password_Hash = PasswordHashing.Hash(user.Password_Hash);
                user.ConfirmPassword = PasswordHashing.Hash(user.ConfirmPassword);

                using (MoviesEntities dc = new MoviesEntities()) { dc.RegUsers.Add(user); dc.SaveChanges(); return RedirectToAction("Login", "Account"); }
            }
            else { ViewBag.Message = "Error while processing your request"; return View(); }
        }

        [NonAction]
        public Boolean IsEmailExists(string EmailId) { using (MoviesEntities dc = new MoviesEntities()) { var mail = dc.RegUsers.Where(a => a.Email == EmailId).FirstOrDefault(); return mail != null; } }

        [HttpGet]
        public ActionResult Login() { if (User.Identity.IsAuthenticated) { return RedirectToAction("Index", "Home"); } else { return View(); } }

        [HttpPost]
        public ActionResult Login(Login login, string ReturnUrl)
        {
            string Message = "";
            if (ModelState.IsValid)
            {
                using (MoviesEntities dc = new MoviesEntities())
                {
                    var email = dc.RegUsers.Where(a => a.Email == login.Email).FirstOrDefault();
                    if (email != null)
                    {
                        if (string.Compare(PasswordHashing.Hash(login.Password), email.Password_Hash) == 0)
                        {
                            int timeout = login.RememberMe ? 525600 : 20;
                            var ticket = new FormsAuthenticationTicket(login.Email, login.RememberMe, timeout);
                            string encrypted = FormsAuthentication.Encrypt(ticket);
                            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                            cookie.Expires = DateTime.Now.AddMinutes(timeout);
                            cookie.HttpOnly = true;
                            Response.Cookies.Add(cookie);

                            Message = "U have successfully signed in";
                            if (Url.IsLocalUrl(ReturnUrl)) { return Redirect(ReturnUrl); }
                            else { return RedirectToAction("Index", "Home"); }
                        }
                        else { Message = "Invalid credential provided"; }
                    }
                    else { Message = "Invalid credential provided"; }
                }
            }
            else { return View(); }

            ViewBag.Message = Message;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Logout() { FormsAuthentication.SignOut(); return RedirectToAction("Index", "Home"); }
    }
}