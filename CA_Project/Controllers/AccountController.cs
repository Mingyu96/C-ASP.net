using CA_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using CA_Project.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CA_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDBContext db;
        private readonly SessionDictionary _sessionDict;
        public AccountController(ApplicationDBContext db, SessionDictionary sessionDict)
        { 
            this.db = db;
            _sessionDict = sessionDict;
        }
        public IActionResult MyAccount() //Account Detail page
        {
            if (CheckLoginStatus())
            {
               
                string ses = Request.Cookies["sessionid"];
                Session session1 = db.Sessions.FirstOrDefault(x =>
             x.Id.ToString() == ses);

                User user = db.Users.FirstOrDefault(x =>
                    x.Id == session1.User.Id
                );
             
                Cart cart1 = db.Carts.FirstOrDefault(x =>
                x.User.Id == session1.User.Id
            );
                ViewData["cart"] = cart1;
                ViewData["account"] = user;
                return View();
            }
            return RedirectToAction("Logout", "Home");
            
        }
        public IActionResult ChangePW() //change Password page
        {
            if (CheckLoginStatus()) { return View(); }

            return RedirectToAction("Logout", "Home");
        }
        public IActionResult Validate([FromBody] User user)//validate old password and update it into new password
        {
            string ses = Request.Cookies["sessionid"];
            Session session1 = db.Sessions.FirstOrDefault(x =>
             x.Id.ToString() == ses);
            User u = db.Users.FirstOrDefault(x =>
                x.Id == session1.User.Id
            );
            if (user.Password == u.Password)
            {
                u.Password = user.Username;
                db.SaveChanges();
                return Json(new { isOkay = true });
            }

            return Json(new { isOkay = false });
        }
        public IActionResult Invalid()
        {
            return View();
        }

        private string GetSessionID()
        {
            return Request.Cookies["sessionid"];
        }

        private bool CheckLoginStatus()
        {
            string sessionid = Request.Cookies["sessionid"];
            if (sessionid == null)
            {
                return false;
            }
            long? time = _sessionDict.CheckSessionPresence(sessionid);
            if (time == null)
            {
                return false;
            }
            else
            {
                //check if time is expired
                if ((time + Session.timeout) < DateTimeOffset.Now.ToUnixTimeSeconds())
                {
                    return false;
                }
            }
            return true;
        }

    }
}
