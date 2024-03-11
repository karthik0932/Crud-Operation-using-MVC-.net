using CrudOperation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CrudOperation.Controllers
{
    public class AccountsController : Controller
    {

        EFCodeFirstEntities entity = new EFCodeFirstEntities();

        // GET: Accounts
        public ActionResult Login()
        {

            return View();
        }
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User credentials)
        {
            bool userExists = entity.Users.Any(x => x.Email == credentials.Email && x.Password == credentials.Password);
            User u = entity.Users.FirstOrDefault(x => x.Email == credentials.Email && x.Password == credentials.Password);
            if (userExists)
            {
                FormsAuthentication.SetAuthCookie(u.Username, false);
                Session["UserLoginID"] = u.UserID;
                return RedirectToAction("Index", "Products");
            }



            ModelState.AddModelError("", "Username or password is wrong");
            return View();
        }

        [HttpPost]
        public ActionResult Signup(User userinfo)
        {
            entity.Users.Add(userinfo);
            entity.SaveChanges();
            return RedirectToAction("Login");
        }
        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


    }
}
        
