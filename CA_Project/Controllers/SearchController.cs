using CA_Project.Models;
using CA_Project.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAShoppingCart.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDBContext shopContext;
        private SessionDictionary _sessionDict;
        public SearchController(ApplicationDBContext shopContext, SessionDictionary sessionDict)
        {
            this.shopContext = shopContext;
            _sessionDict = sessionDict;
        }

        public IActionResult Index(string searchStr)//receive search string and search product with match string
        {
            if (!CheckLoginStatus())
            {
                return LocalRedirect("/home/index");
            }
            if (searchStr == null)
            {
                searchStr = "";
            }

            List<Product> products = shopContext.Products.Where(x =>
                x.ProductName.Contains(searchStr) || x.Desc.Contains(searchStr)
            ).ToList();

            Session sess1 = shopContext.Sessions.FirstOrDefault(x =>
                x.Id.ToString() == Request.Cookies["sessionid"]
            );
            if(sess1 == null)
            {
                return LocalRedirect("/home/index");
            }
            Cart cart1 = shopContext.Carts.FirstOrDefault(x =>
                x.User.Id == sess1.User.Id
            );
            if(cart1 == null)
            {
                ViewData["cart"] = null;
            }
            ViewData["cart"] = cart1;
            ViewData["searchStr"] = searchStr;
            ViewData["products"] = products;

            return View();
        }

        private bool CheckLoginStatus()// Check session for log in status
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
