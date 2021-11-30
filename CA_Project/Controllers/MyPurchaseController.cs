using Microsoft.AspNetCore.Mvc;
using CA_Project.Data;
using System;
using CA_Project.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace CA_Project.Controllers
{
    public class MyPurchaseController : Controller
    {
        private readonly ApplicationDBContext db;
        private readonly SessionDictionary _sessionDict;
        public MyPurchaseController(ApplicationDBContext db, SessionDictionary sessionDict)
        {
            this.db = db;
            _sessionDict = sessionDict;
        }
        public IActionResult Record()
        {
            if (!CheckLoginStatus())
            {
                return LocalRedirect("/home/index");
            }
            string sessionid=GetSessionID();// Get current session id

            Session session = db.Sessions.FirstOrDefault(x =>
               x.Id.ToString() == sessionid //find session with matched session id
           );

            List <Purchase> purchase = db.Purchases.Where(x =>
            x.User.Id ==session.User.Id).ToList(); //List of purchase record with matched user id

            List <PurchaseProduct> Detail = db.PurchaseProducts.Where(x => x.Purchase.User.Id ==session.User.Id).ToList(); // list of purchase product with matched user Id

            Cart cart1 = db.Carts.FirstOrDefault(x =>  // Get cart with matched user Id
            x.User.Id == session.User.Id    
        );
            ViewData["cart"] = cart1;
            ViewData["Purchase"] = purchase;
            ViewData["Detail"] = Detail;
            return View();
        }
        private string GetSessionID()
        {
            return Request.Cookies["sessionid"];
        }

        private bool CheckLoginStatus()
        {
            string sessionid = GetSessionID();
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
    } }
