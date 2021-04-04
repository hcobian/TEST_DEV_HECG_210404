using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace WEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string Message)
        {
            if(User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Customers");
            ViewBag.message = Message;
            return View("Index");
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var user = WebConfigurationManager.AppSettings["user"];
                var pass = WebConfigurationManager.AppSettings["pass"];
                if (user == email && pass == password)
                {
                    FormsAuthentication.SetAuthCookie(email,true);
                    return RedirectToAction("Index","Customers");
                }
                else
                {
                    return Index("No se encontro el usuario");
                }
            }
            else 
            {
                return Index("llena los campos");
            }
        }
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return View("Index");
        }

    }
}